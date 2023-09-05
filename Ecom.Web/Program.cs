using ECOM.Web.Services;
using ECOM.Web.Services.IService;
using ECOM.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IShoppingCartService, ShoppingCartService>();

// sign in
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.LoginPath = "/Auth/login";
    options.AccessDeniedPath = "/auth/denied";
});


builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();


StaticDetails.CouponAPIBase = builder.Configuration["ServiceURLs:CouponAPI"];
StaticDetails.AuthAPIBase = builder.Configuration["ServiceURLs:AuthAPI"];
StaticDetails.ProductAPIBase = builder.Configuration["ServiceURLs:ProductAPI"];
StaticDetails.ShoppingCartAPIBase = builder.Configuration["ServiceURLs:ShoppingCartAPI"];


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
