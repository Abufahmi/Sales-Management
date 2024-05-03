using System.ComponentModel.DataAnnotations;

namespace Sales.Library
{
    public class Role
    {
        [Key] public string? Id { get; set; }
        [Required] public string? RoleName { get; set; }
    }
}
