using System.Text.Json;

namespace Sales.Client.Services;

public class Serialization
{
    public static T? DeSerializeJsonString<T>(string jsonString)
    {
        return JsonSerializer.Deserialize<T>(jsonString);
    }

    public static string SerializeObj<T>(T model)
    {
        return JsonSerializer.Serialize(model);
    }
}
