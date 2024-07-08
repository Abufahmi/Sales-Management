using System.ComponentModel.DataAnnotations;

namespace Sales.Library
{
    public class Category
    {
        [Key] public int Id { get; set; }
        [Required] public string? Name { get; set; }
        public string? Image { get; set; }
    }
}
