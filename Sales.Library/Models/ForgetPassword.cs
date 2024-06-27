using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Models
{
    public class ForgetPassword
    {
        [Required, DataType(DataType.EmailAddress)] public string? EmailAddress { get; set; }
        [Required] public EmailModel? EmailModel { get; set; }
    }
}
