

using AutoMapper;
using ECOM.Services.OrderAPI.Data;
using ECOM.Services.OrderAPI.Extension;
using ECOM.Services.OrderAPI.Models;
using ECOM.Services.OrderAPI.Models.DTO;
using ECOM.Services.OrderAPI.Utility;
using ECOM.Services.ShoppingOrderAPI;
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
IMapper _mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(_mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//for autherizing inter service calls
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackendAPIAuthenticationHttpHandler>();

builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(builder.Configuration["ServiceURLs:ProductAPI"])).AddHttpMessageHandler<BackendAPIAuthenticationHttpHandler>();
builder.Services.AddHttpClient("Coupon", u => u.BaseAddress = new Uri(builder.Configuration["ServiceURLs:CouponAPI"])).AddHttpMessageHandler<BackendAPIAuthenticationHttpHandler>();
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<ICouponService, CouponService>();
//builder.Services.AddScoped<IMessageBus, MessageBus>();
//
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


app.MapPost("/CreateOrder", async (AppDBContext dBContext, [FromBody] ShoppingCartDTO shoppingCart) =>
{
    try
    {
        OrderHeaderDTO orderHeader = _mapper.Map<OrderHeaderDTO>(shoppingCart.CartHeader);
        orderHeader.OrderTime = DateTime.Now;
        orderHeader.Status = SD.Status_Pending;

        // create order details 
        orderHeader.OrderDetails = _mapper.Map<IEnumerable<OrderDetailDTO>>(shoppingCart.CartDetails);

        OrderHeader createdOrder = dBContext.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeader)).Entity;
        await dBContext.SaveChangesAsync(true);
        orderHeader.OrderHeaderId = createdOrder.OrderHeaderID;

        return new ResponseDTO
        {
            Message = "Succussfully upserted",
            Result = orderHeader
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


}).WithName("CreateOrder").RequireAuthorization().WithOpenApi();
app.Run();