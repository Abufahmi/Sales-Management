using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sales.Library
{
    public class UserLogin
    {
        [Key] public string? Id { get; set; }

        [Required] public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))] public User? User { get; set; }

        [Required]public DateTime LoginDate { get; set; }
        public DateTime LogoutDate { get; set; }
        [Required] public string? RefreshToken { get; set; }
    }
}
