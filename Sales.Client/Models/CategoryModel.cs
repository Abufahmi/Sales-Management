using Sales.Client.Resources;
using System.ComponentModel.DataAnnotations;

namespace Sales.Client.Models;

public class CategoryModel
{
    public int Id { get; set; }

    [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.NameRequired),
       ErrorMessageResourceType = typeof(ErrorResourceMessage))]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.ImageRequired),
       ErrorMessageResourceType = typeof(ErrorResourceMessage))]
    public string Image { get; set; } = string.Empty;

    public bool IsSelected { get; set; }
}
