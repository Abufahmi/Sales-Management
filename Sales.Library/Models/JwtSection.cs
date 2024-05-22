using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Models
{
    public class JwtSection
    {
        [Required] public string? Key { get; set; }
        [Required] public string? Issuer { get; set; }
        [Required] public string? Audience { get; set; }
    }
}
