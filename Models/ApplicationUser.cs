
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SecureShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? FullName { get; set; }
    }
}
