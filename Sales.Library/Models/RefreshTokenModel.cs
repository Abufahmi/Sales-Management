using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Models
{
    public class RefreshTokenModel
    {
        [Required] public string? RefreshToken { get; set; }
    }
}
