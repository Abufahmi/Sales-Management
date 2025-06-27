using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Entities
{
    public class UserLogin
    {
        [Key] public string Id { get; set; } = string.Empty;

        [Required] public string UserId { get; set; } = string.Empty;   
        [ForeignKey(nameof(UserId))] public User? User { get; set; }

        [Required]public DateTime LoginDate { get; set; }
        public DateTime LogoutDate { get; set; }
        [Required] public string RefreshToken { get; set; } = string.Empty;
    }
}
