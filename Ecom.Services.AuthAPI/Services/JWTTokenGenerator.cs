using ECOM.Services.AuthAPI.Models;
using ECOM.Services.AuthAPI.Services.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECOM.Services.AuthAPI.Services;

public class JWTTokenGenerator : IJWTTokenGenerator
{
    private readonly JWTOptions _jWTOptions;
    public JWTTokenGenerator( IOptions<JWTOptions> jWTOptions)
    {

        _jWTOptions = jWTOptions.Value;

    }
    public string GenerateToken(ApplicationUser applicationUser)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jWTOptions.Secret);
        var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Email, applicationUser.Name), new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id), new Claim(JwtRegisteredClaimNames.Name, applicationUser.Name) };
        var TokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jWTOptions.Audience,
            Issuer = _jWTOptions.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Today.AddDays(1)
        };
        var token = tokenHandler.CreateJwtSecurityToken(TokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
