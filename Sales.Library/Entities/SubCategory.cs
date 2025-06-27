using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Library.Entities
{
    public class SubCategory
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))] public Category? Category { get; set; }
    }
}
