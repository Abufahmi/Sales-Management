using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Library.Entities
{
    public class Notification
    {
        [Key] public string Id { get; set; } = string.Empty;
        [Required] public string Message { get; set; } = string.Empty;
        [Required] public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))] public User? User { get; set; }
        [Required] public DateTime? CreatedDate { get; set; }
    }
}
