using Microsoft.AspNetCore.Identity;

namespace WebApiIdentity.Identity
{
    public class AppUser:IdentityUser
    {
        public string? Id { get; set; }
    }
}
