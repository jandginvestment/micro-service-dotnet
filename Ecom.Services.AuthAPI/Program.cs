using ECOM.Services.AuthAPI.Data;
using ECOM.Services.AuthAPI.Models;
using ECOM.Services.AuthAPI.Models.DTO;
using ECOM.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECOM.Services.AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDBContext>(option => { option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });

//Identity related 

builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("APISettings:JWTOptions"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IJWTTokenGenerator, JWTTokenGenerator>();
builder.Services.AddScoped<JWTTokenGenerator>();

builder.Services.AddScoped<IAuthService, AuthService>();
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


#region API
app.MapPost("/api/auth/register", async (IAuthService authService, [FromBody] RegistrationRequestDTO registrationRequest) =>
{

    var error = await authService.Register(registrationRequest);

    if (!string.IsNullOrEmpty(error))
    {
        return Results.BadRequest(new ResponseDTO() { Error = error, IsSuccess = false });
    }
    return Results.Ok(new ResponseDTO() { IsSuccess = true, Message = "Successfully created" });

}).WithName("Register").WithOpenApi();

app.MapPost("/api/auth/login", async (IAuthService _authService, [FromBody] LoginRequestDTO loginRequest) =>
{
    var loginResponse = await _authService.Login(loginRequest);

    if(loginResponse.User == null) { return Results.BadRequest(new ResponseDTO() { Message = "user name or password are incorrect", IsSuccess = false }); }

    return Results.Ok(new ResponseDTO() { Result = loginResponse });

}).WithName("Login").WithOpenApi();


app.MapPost("/api/auth/assignRole", async (IAuthService _authService, [FromBody] RegistrationRequestDTO registrationRequest) =>
{
    var roleAssignment = await _authService.AssignRole(registrationRequest.Email, registrationRequest.Role.ToUpper());

    if (!roleAssignment) { return Results.BadRequest(new ResponseDTO() { Message = "Role error encountered", IsSuccess = false }); }

    return Results.Ok(new ResponseDTO() { Result = roleAssignment ,IsSuccess = true});

}).WithName("assignRole").WithOpenApi();
#endregion

app.Run();


/*                                                     */




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