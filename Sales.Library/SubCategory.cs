using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Library
{
    public class SubCategory
    {
        [Key] public int Id { get; set; }
        [Required] public string? Name { get; set; }
        [Required] public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))] public Category? Category { get; set; }
    }
}
