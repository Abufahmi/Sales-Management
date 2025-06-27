using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Library.Entities
{
    public class Verification
    {
        [Key] public string Token { get; set; } = string.Empty;
        [Required] public string VerificationCode { get; set; } = string.Empty;
        [Required] public DateTime CreatedDate { get; set; }

        [Required] public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))] public User? User { get; set; }
    }
}
