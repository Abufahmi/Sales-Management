using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Entities
{
    public class Role
    {
        [Key] public string Id { get; set; } = string.Empty;
        [Required] public string RoleName { get; set; } = string.Empty;
    }
}
