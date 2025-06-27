using Sales.Client.Resources;
using System.ComponentModel.DataAnnotations;

namespace Sales.Client.Models.SettingModels
{
    public class UserModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.UserNameRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.EmailAddressRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.PasswordRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string Password { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool PhoneConfirmed { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public string? Image { get; set; }

        public bool IsSelected { get; set; }
    }
}
