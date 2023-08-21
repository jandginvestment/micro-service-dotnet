using AutoMapper;
using ECOM.Services.CouponAPI;
using ECOM.Services.CouponAPI.Data;
using ECOM.Services.CouponAPI.Models;
using ECOM.Services.CouponAPI.Models.DTO;
using ECOM.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            Description = "Enter the token here", In=ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,Scheme = JwtBearerDefaults.AuthenticationScheme
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

var settingSection = builder.Configuration.GetSection("APISettings");
var secret = settingSection.GetValue<string>("Secret");
var issuer = settingSection.GetValue<string>("Issuer");
var audience = settingSection.GetValue<string>("Audience");
var key = Encoding.ASCII.GetBytes(secret);


builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(j =>
{
    j.TokenValidationParameters = new TokenValidationParameters
    { ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(key), ValidateIssuer = true ,ValidIssuer = issuer,ValidateAudience = true,ValidAudience = audience};
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    ApplyMigration();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

// Web methods/API declarations

// Get all coupons


app.Map("/Coupons", cpn =>
{

    app.MapGet("/Get", (AppDBContext dBContext) =>
    {
        try
        {
            IEnumerable<Coupon> coupons = dBContext.Coupons.ToList();

            var response = new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<IEnumerable<CouponDTO>>(coupons)
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

    }).RequireAuthorization().WithName("getCoupons").WithOpenApi();

    // Get coupon by ID
    app.MapGet("/Get/{id}", (AppDBContext dBContext, int id) =>
    {
        try
        {
            Coupon coupon = dBContext.Coupons.First(u => u.CouponId == id);
            return new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<CouponDTO>(coupon)
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

    }).WithName("getCouponsByID").WithOpenApi();

    // Get coupon by code
    app.MapGet("/GetByCode/{code}", (AppDBContext dBContext, string code) =>
    {
        try
        {
            Coupon coupon = dBContext.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower());
            return new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<CouponDTO>(coupon)
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

    }).WithName("getCouponsByCode").WithOpenApi();

    // Post a new coupon
    app.MapPost("/Post", (AppDBContext dBContext, [FromBody] CouponDTO couponDTO) =>
    {
        try
        {
            var coupon = mapper.Map<Coupon>(couponDTO);

            dBContext.Coupons.Add(coupon);
            dBContext.SaveChanges();
            return new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<CouponDTO>(coupon),
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

    }).WithName("PostCoupon").WithOpenApi();

    // Update an existing coupon
    app.MapPut("/Put", (AppDBContext dBContext, [FromBody] CouponDTO couponDTO) =>
    {
        try
        {
            var coupon = mapper.Map<Coupon>(couponDTO);
            dBContext.Coupons.Update(coupon);
            dBContext.SaveChanges();
            return new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<CouponDTO>(coupon),
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

    }).WithName("UpdateCoupon").WithOpenApi();

    // Delete a coupon
    app.MapDelete("/Delete/{couponID}", (AppDBContext dBContext, int couponID) =>
    {
        try
        {
            Coupon coupon = dBContext.Coupons.First(u => u.CouponId == couponID);
            dBContext.Coupons.Remove(coupon);
            dBContext.SaveChanges();
            return new ResponseDTO
            {
                IsSuccess = true,
                Result = mapper.Map<CouponDTO>(coupon),
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

    }).WithName("DeleteCoupon").WithOpenApi();
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