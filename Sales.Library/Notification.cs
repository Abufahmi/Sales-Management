using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Library
{
    public class Notification
    {
        [Key] public string? Id { get; set; }
        [Required] public string? Message { get; set; }
        [Required] public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))] public User? User { get; set; }
        [Required] public DateTime? CreatedDate { get; set; }
    }
}
