using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Library.Entities
{
    public class UserRole
    {
        [Key] public string Id { get; set; } = string.Empty;

        [Required] public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))] public User? User { get; set; }

        [Required] public string RoleId { get; set; } = string.Empty;
        [ForeignKey(nameof(RoleId))] public Role? Role { get; set; }
    }
}
