using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Library
{
    public class UserRole
    {
        [Key] public string? Id { get; set; }

        [Required] public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))] public User? User { get; set; }

        [Required] public string? RoleId { get; set; }
        [ForeignKey(nameof(RoleId))] public Role? Role { get; set; }
    }
}
