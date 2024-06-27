using Sales.Client.Resources;
using System.ComponentModel.DataAnnotations;

namespace Sales.Client.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.VerificationCodeRequired),
          ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string? VerificationCode { get; set; }

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.PasswordRequired),
           ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$",
           ErrorMessageResourceName = nameof(ErrorResourceMessage.PasswordNotValid),
           ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string? Password { get; set; }

        [Compare(nameof(Password), 
            ErrorMessageResourceName = nameof(ErrorResourceMessage.PasswordNotMatch),
            ErrorMessageResourceType = typeof(ErrorResourceMessage)), DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.PasswordConfirmRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string? PasswordConfirm { get; set; }
    }
}
