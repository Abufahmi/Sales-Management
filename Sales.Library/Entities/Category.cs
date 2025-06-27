using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Entities
{
    public class Category
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Image { get; set; } = string.Empty;
    }
}
