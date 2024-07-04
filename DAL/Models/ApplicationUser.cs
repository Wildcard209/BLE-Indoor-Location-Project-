using Microsoft.AspNetCore.Identity;
using Shared.Enums;

namespace DAL.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public required string DisplayName { get; set; }
    }
}
