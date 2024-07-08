using Sales.Client.Models.SettingModels;
using Sales.Library;

namespace Sales.Client.Services
{
    public class ModelConverter
    {
        internal static List<UserModel>? GetUserModels(List<User> users)
        {
            var models = new List<UserModel>();
            foreach (var item in users)
            {
                if (item == null) continue;
                var model = new UserModel
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    Password = item.Password,
                    PhoneNumber = item.PhoneNumber,
                    EmailConfirmed = item.EmailConfirmed,
                    PhoneConfirmed = item.PhoneConfirmed,
                    CreatedDate = item.CreatedDate,
                    Image = item.Image,
                    IsSelected = false,
                };
                models.Add(model);
            }
            return models;
        }
    }
}
