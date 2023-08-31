using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using static System.Text.Encoding;

namespace ECOM.Services.ShoppingCartAPI.Extension;

public static class AddAuthenticationExtension
{
    public static WebApplicationBuilder AddAuthenticationBuilder(this WebApplicationBuilder builder)
    {
        var settingSection = builder.Configuration.GetSection("APISettings");
        var secret = settingSection.GetValue<string>("Secret");
        var issuer = settingSection.GetValue<string>("Issuer");
        var audience = settingSection.GetValue<string>("Audience");
        var key = ASCII.GetBytes(secret);


        builder.Services.AddAuthentication(a =>
        {
            a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(j =>
        {
            j.TokenValidationParameters = new TokenValidationParameters
            { ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(key), ValidateIssuer = true, ValidIssuer = issuer, ValidateAudience = true, ValidAudience = audience };
        });

        return builder;
    }
}
