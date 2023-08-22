using AutoMapper;
using ECOM.Services.AuthAPI.Models.DTO;
using ECOM.Services.ProductAPI;
using ECOM.Services.ProductAPI.Data;
using ECOM.Services.ProductAPI.Extension;
using ECOM.Services.ProductAPI.Models;
using ECOM.Services.ProductAPI.Models.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    option =>
    {
        option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter the token here",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                } , new string[]{}
            }
        });

    });

builder.Services.AddDbContext<AppDBContext>(option => { option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
// Auto mapper related
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.AddAuthenticationBuilder();

builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("ADMIN"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

 ApplyMigration();


app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
// Web methods/API declarations

app.Map("/Products", pts =>
{

    app.MapGet("/Get", (AppDBContext dBContext) =>
    {
        try
        {
            IEnumerable<Product> products = dBContext.Products.ToList();

            var response = new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<IEnumerable<ProductDTO>>(products)
            };
            return response;
        }
        catch (Exception e)
        {
            var response = new ResponseDTO
            {
                IsSuccess = false,
                Error = e.Message
            };
            return response;
        }

        return null;

    }).RequireAuthorization().WithName("getProducts").WithOpenApi();

    // Get coupon by ID
    app.MapGet("/Get/{id}", (AppDBContext dBContext, int id) =>
    {
        try
        {
            Product product = dBContext.Products.First(u => u.ProductId == id);
            return new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<ProductDTO>(product)
            };
        }
        catch (Exception e)
        {
            var response = new ResponseDTO
            {
                IsSuccess = false,
                Error = e.Message
            };
            return response;
        }

        return null;

    }).RequireAuthorization().WithName("getProductByID").WithOpenApi();

    
    // Post a new coupon
    app.MapPost("/Post", (AppDBContext dBContext, [FromBody] ProductDTO productDTO) =>
    {
        try
        {
            var product = mapper.Map<Product>(productDTO);

            dBContext.Products.Add(product);
            dBContext.SaveChanges();
            return new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<ProductDTO>(product),
                Message = "Created Successfully"
            };
        }
        catch (Exception e)
        {
            var response = new ResponseDTO
            {
                IsSuccess = false,
                Error = e.Message
            };
            return response;
        }

        return null;

    }).WithName("PostProduct").RequireAuthorization("RequireAdminRole").WithOpenApi();

    // Update an existing coupon
    app.MapPut("/Put", (AppDBContext dBContext, [FromBody] ProductDTO productDTO) =>
    {
        try
        {
            var product = mapper.Map<Product>(productDTO);
            dBContext.Products.Update(product);
            dBContext.SaveChanges();
            return new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<ProductDTO>(product),
                Message = "Updated Successfully"
            };
        }
        catch (Exception e)
        {
            var response = new ResponseDTO
            {
                IsSuccess = false,
                Error = e.Message
            };
            return response;
        }

        return null;

    }).WithName("UpdateProduct").WithOpenApi().RequireAuthorization("RequireAdminRole");

    // Delete a coupon
    app.MapDelete("/Delete/{productID}", (AppDBContext dBContext, int productID) =>
    {
        try
        {
            Product product = dBContext.Products.First(u => u.ProductId == productID);
            dBContext.Products.Remove(product);
            dBContext.SaveChanges();
            return new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<ProductDTO>(product),
                Message = "Deleted Successfully"
            };
        }
        catch (Exception e)
        {
            var response = new ResponseDTO
            {
                IsSuccess = false,
                Error = e.Message
            };
            return response;
        }

        return null;

    }).WithName("DeleteProduct").WithOpenApi().RequireAuthorization("RequireAdminRole");


});

app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDBContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}