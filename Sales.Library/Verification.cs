using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Library
{
    public class Verification
    {
        [Key] public string? Token { get; set; }
        [Required] public string? VerificationCode { get; set; }
        [Required] public DateTime CreatedDate { get; set; }

        [Required] public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))] public User? User { get; set; }
    }
}
