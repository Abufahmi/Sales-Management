using System.ComponentModel.DataAnnotations;

namespace Sales.Library.Entities
{
    public class MainSetting
    {
        [Key] public int Id { get; set; }
        [Required] public int TokenExpireMinutes { get; set; }
        [Required] public int ItemPerPage { get; set; }
    }
}
