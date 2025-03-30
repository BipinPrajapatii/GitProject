using Microsoft.AspNetCore.Identity;

namespace ProjectApplication.Entities
{
    public class MyApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
