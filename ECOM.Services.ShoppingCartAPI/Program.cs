using AutoMapper;
using Ecom.MessageBus;
using Ecom.MessageBus.Interfaces;
using ECOM.Services.ShoppingCartAPI;
using ECOM.Services.ShoppingCartAPI.Data;
using ECOM.Services.ShoppingCartAPI.Extension;
using ECOM.Services.ShoppingCartAPI.Models;
using ECOM.Services.ShoppingCartAPI.Models.DTO;
using ECOM.Services.ShoppingCartAPI.Services;
using ECOM.Services.ShoppingCartAPI.Services.IServices;
using ECOM.Services.ShoppingCartAPI.Utility;
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

//for autherizing inter service calls
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackendAPIAuthenticationHttpHandler>();

builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(builder.Configuration["ServiceURLs:ProductAPI"])).AddHttpMessageHandler<BackendAPIAuthenticationHttpHandler>();
builder.Services.AddHttpClient("Coupon", u => u.BaseAddress = new Uri(builder.Configuration["ServiceURLs:CouponAPI"])).AddHttpMessageHandler<BackendAPIAuthenticationHttpHandler>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

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

app.Map("/ShoppingCart", sc =>
{
    // Post a new shopping cart
    app.MapPost("/Upsert", async (AppDBContext dBContext, ShoppingCartDTO shoppingCart) =>
    {
        try
        {
            var cartHeaderFromDB = await dBContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserID == shoppingCart.CartHeader.UserID);
            if (cartHeaderFromDB != null)
            {
                // update cart header
                var cartDetailsFromDB = await dBContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                     d => d.ProductID == shoppingCart.CartDetails.First().ProductID &&
                     d.CartHeaderID == cartHeaderFromDB.CartHeaderID
                    );
                if (cartDetailsFromDB != null)
                {
                    // update count
                    IEnumerable<CartDetail> cartDetails = mapper.Map<IEnumerable<CartDetail>>(shoppingCart.CartDetails);

                    foreach (var cartDetail in cartDetails)
                    {
                        cartDetail.CartHeaderID = cartDetailsFromDB.CartHeaderID;
                        cartDetail.CartDetailID = cartDetailsFromDB.CartDetailID;
                        if (cartDetail.ProductID == cartDetailsFromDB.ProductID)
                        {
                            cartDetail.Count += cartDetailsFromDB.Count;
                        }
                    }

                    dBContext.CartDetails.UpdateRange(cartDetails);
                    await dBContext.SaveChangesAsync(true);
                }
                else
                {
                    // create cart details 
                    IEnumerable<CartDetail> cartDetails = mapper.Map<IEnumerable<CartDetail>>(shoppingCart.CartDetails);

                    foreach (var cartDetail in cartDetails)
                    {
                        cartDetail.CartHeaderID = cartHeaderFromDB.CartHeaderID;
                    }

                    dBContext.CartDetails.AddRange(cartDetails);
                    await dBContext.SaveChangesAsync(true);
                }

            }
            else
            {
                // create cart header and details

                CartHeader cartHeader = mapper.Map<CartHeader>(shoppingCart.CartHeader);
                dBContext.CartHeaders.Add(cartHeader);
                await dBContext.SaveChangesAsync();

                IEnumerable<CartDetail> cartDetails = mapper.Map<IEnumerable<CartDetail>>(shoppingCart.CartDetails);

                foreach (var cartDetail in cartDetails)
                {
                    cartDetail.CartHeaderID = cartHeader.CartHeaderID;
                }

                dBContext.CartDetails.AddRange(cartDetails);
                await dBContext.SaveChangesAsync(true);


            }

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

        return new ResponseDTO
        {
            Message = "Succussfully upserted",
            Result = shoppingCart
        };

    }).WithName("CartUpsert").RequireAuthorization().WithOpenApi();
    app.MapPost("/ApplyCoupon", async (AppDBContext dBContext, [FromBody] ShoppingCartDTO shoppingCart) =>
    {

        try
        {
            var cartFromDB = await dBContext.CartHeaders.FirstAsync<CartHeader>(h => h.UserID == shoppingCart.CartHeader.UserID);

            cartFromDB.CouponCode = shoppingCart.CartHeader.CouponCode;
            dBContext.CartHeaders.Update(cartFromDB);
            await dBContext.SaveChangesAsync(true);
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
        return new ResponseDTO
        {
            Message = "Coupon code Succussfully applied",
        };
    }).WithName("ApplyCoupon").RequireAuthorization().WithOpenApi();
    app.MapPost("/RemoveCoupon", async (AppDBContext dBContext, [FromBody] ShoppingCartDTO shoppingCart) =>
    {

        try
        {
            var cartFromDB = await dBContext.CartHeaders.FirstAsync<CartHeader>(h => h.UserID == shoppingCart.CartHeader.UserID);

            cartFromDB.CouponCode = string.Empty;
            dBContext.CartHeaders.Update(cartFromDB);
            await dBContext.SaveChangesAsync(true);
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
        return new ResponseDTO
        {
            Message = "Coupon code Succussfully removed",
        };
    }).WithName("RemoveCoupon").RequireAuthorization().WithOpenApi();

    app.MapPost("/Remove", async (AppDBContext dBContext, [FromBody] int shoppingCartDetailID) =>
    {
        try
        {
            CartDetail cartDetail = dBContext.CartDetails.First(d => d.CartDetailID == shoppingCartDetailID);
            dBContext.CartDetails.Remove(cartDetail);

            int totalCartCount = dBContext.CartDetails.Where(d => d.CartHeaderID == cartDetail.CartHeaderID).Count();

            if (totalCartCount == 1)
            {
                var cartHeaderToRemove = await dBContext.CartHeaders.FirstOrDefaultAsync(d => d.CartHeaderID == cartDetail.CartHeaderID);
                dBContext.CartHeaders.Remove(cartHeaderToRemove);
            }
            await dBContext.SaveChangesAsync(true);

        }
        catch (Exception e)
        {

            return new ResponseDTO { IsSuccess = false, Error = e.Message };
        }
        return new ResponseDTO { Message = "Cart removed successfully" };
    }).WithName("RemoveCart").RequireAuthorization().WithOpenApi();
    app.MapGet("/Get/{userID}", async (AppDBContext dBContext, ICouponService _CouponService, IProductService _ProductService, string userID) =>
    {
        try
        {
            ShoppingCartDTO shoppingCart = new ShoppingCartDTO()
            {
                CartHeader = mapper.Map<CartHeaderDTO>(dBContext.CartHeaders.FirstOrDefault(h => h.UserID == userID)),

            };
            IEnumerable<ProductDTO> products = await _ProductService.GetProducts();
            shoppingCart.CartDetails = mapper.Map<IEnumerable<CartDetailDTO>>(dBContext.CartDetails.Where(d => d.CartHeaderID == shoppingCart.CartHeader.CartHeaderID));


            foreach (var cartDetail in shoppingCart.CartDetails)
            {
                cartDetail.Product = products.FirstOrDefault(p => p.ProductID == cartDetail.ProductID);
                shoppingCart.CartHeader.CartTotal += Math.Round(cartDetail.Count * cartDetail.Product.Price, 2);
            }
            // apply discount if any coupon code on cart header
            if (!string.IsNullOrEmpty(shoppingCart.CartHeader.CouponCode))
            {
                var coupon = await _CouponService.getCouponByCode(shoppingCart.CartHeader.CouponCode);
                if (coupon != null && shoppingCart.CartHeader.CartTotal > coupon.MinimumAmount)
                {
                    shoppingCart.CartHeader.CartTotal -= coupon.DiscountAmount;
                    shoppingCart.CartHeader.Discount = coupon.DiscountAmount;
                }
            }
            return new ResponseDTO { Result = shoppingCart };
        }
        catch (Exception e)
        {
            return new ResponseDTO { IsSuccess = false, Error = e.Message };
        }


    }

    ).WithName("GetCart").RequireAuthorization().WithOpenApi();

    app.MapPost("/EmailCart", async ([FromBody] ShoppingCartDTO shoppingCart, IMessageBus _MessageBus, IConfiguration _configuration) =>
    {

        try
        {
            await _MessageBus.PublishMessage(shoppingCart, _configuration.GetValue<string>("TopicAndQueNames:EmailShoppingCart"));
            return new ResponseDTO { Result = shoppingCart };
        }
        catch (Exception e)
        {

            return new ResponseDTO { IsSuccess = false, Error = e.Message };

        }


    }).WithName("EmailCart").RequireAuthorization().WithOpenApi();
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

