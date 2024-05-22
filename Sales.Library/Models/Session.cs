using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Models
{
    public class Session
    {
        [Required] public string? UserId { get; set; }
        [Required] public string? UserName { get; set; }
        [Required] public string? Email { get; set; }
        [Required] public string? RoleName { get; set; }
    }
}