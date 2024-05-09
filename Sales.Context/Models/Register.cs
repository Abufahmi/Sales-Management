using System.ComponentModel.DataAnnotations;

namespace Sales.Context.Models
{
    public class Register
    {
        [Required] public string? UserName { get; set; }

        [Required]
        [RegularExpression(@"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$", ErrorMessage = "Email not valid")]
        public string? Email { get; set; }

        [Required] public string? Password { get; set; }
    }
}
