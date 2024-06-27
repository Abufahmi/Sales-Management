using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Models
{
    public class EmailModel
    {
        [Required] public string? Subject { get; set; }
        [Required] public string? Body { get; set; }
        [Required] public string? VerifyCode { get; set; }
    }
}
