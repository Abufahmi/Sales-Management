using Sales.Client.Resources;
using System.ComponentModel.DataAnnotations;

namespace Sales.Client.Models
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.UserNameRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string? UserName { get; set; }

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.PasswordRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))] 
        public string? Password { get; set; }
    }
}
