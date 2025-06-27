using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sales.Client.Resources;
using Sales.Library.Entities;

namespace Sales.Client.Models.SettingModels
{
    public class UserRoleModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.UserRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))] public User? User { get; set; }

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.RoleRequired),
             ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public string RoleId { get; set; } = string.Empty;
        [ForeignKey(nameof(RoleId))] public Role? Role { get; set; }

        public bool IsSelected { get; set; }    
    }
}
