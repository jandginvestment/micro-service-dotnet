using Microsoft.AspNetCore.Identity;

namespace ECOM.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
