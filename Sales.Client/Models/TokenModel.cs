using System.ComponentModel.DataAnnotations;

namespace Sales.Client.Models
{
    public class TokenModel
    {
        [Required] public string? Token { get; set; }
    }
}
