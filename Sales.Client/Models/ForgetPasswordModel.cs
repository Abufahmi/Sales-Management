using Sales.Client.Resources;
using Sales.Library.Models;
using System.ComponentModel.DataAnnotations;

namespace Sales.Client.Models
{
    public class ForgetPasswordModel
    {
        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.EmailAddressRequired),
           ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
           ErrorMessageResourceName = nameof(ErrorResourceMessage.EmailAddressNotValid),
           ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string? Email { get; set; }

        public EmailModel? EmailModel { get; set; }
    }
}
