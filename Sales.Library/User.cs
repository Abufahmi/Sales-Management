using System.ComponentModel.DataAnnotations;

namespace Sales.Library
{
    public class User
    {
        [Key] public string? Id { get; set; }
        [Required] public string? UserName { get; set; }
        [Required] public string? Email { get; set; }
        [Required] public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneConfirmed { get; set; }
        [Required] public DateTime CreatedDate { get; set; }
        public string? Image { get; set; }
    }
}
