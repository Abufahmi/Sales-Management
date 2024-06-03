using Sales.Client.Resources;
using System.ComponentModel.DataAnnotations;

namespace Sales.Client.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.UserNameRequired),
           ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string? UserName { get; set; }

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.EmailAddressRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessageResourceName = nameof(ErrorResourceMessage.EmailAddressNotValid),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string? Email { get; set; }

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.PasswordRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$",
            ErrorMessageResourceName = nameof(ErrorResourceMessage.PasswordNotValid),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessageResourceName = nameof(ErrorResourceMessage.PasswordNotMatch),
            ErrorMessageResourceType = typeof(ErrorResourceMessage)), DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.PasswordConfirmRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string? PasswordConfirm { get; set; }
    }
}
