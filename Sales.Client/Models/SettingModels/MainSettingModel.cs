using Sales.Client.Resources;
using System.ComponentModel.DataAnnotations;

namespace Sales.Client.Models.SettingModels
{
    public class MainSettingModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.TokenExpireMinutesRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        [Range(1, 1000000000, ErrorMessageResourceName = nameof(ErrorResourceMessage.TokenExpireMinutesRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public int TokenExpireMinutes { get; set; }

        [Required(ErrorMessageResourceName = nameof(ErrorResourceMessage.ItemPerPageRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        [Range(1, 1000000000, ErrorMessageResourceName = nameof(ErrorResourceMessage.ItemPerPageRequired),
            ErrorMessageResourceType = typeof(ErrorResourceMessage))]
        public int ItemPerPage { get; set; }
    }
}
