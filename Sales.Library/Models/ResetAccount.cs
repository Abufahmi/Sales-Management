using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Models
{
    public class ResetAccount
    {
        [Required] public string? Password { get; set; }
        [Required] public string? UserId { get; set; }
    }
}
