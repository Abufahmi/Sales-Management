using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Entities
{
    public class User
    {
        [Key] public string Id { get; set; } = string.Empty;
        [Required] public string UserName { get; set; } = string.Empty;
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneConfirmed { get; set; }
        [Required] public DateTime CreatedDate { get; set; }
        public string? Image { get; set; }
    }
}
