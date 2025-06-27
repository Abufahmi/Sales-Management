using System.Web;

namespace Sales.Library.Services;

public class SerializationService<T>
{
    public static List<T>? DeSerializeList(string serializedList)
    {
        return System.Text.Json.JsonSerializer.Deserialize<List<T>>(HttpUtility.UrlDecode(serializedList));
    }

    public static T? DeSerializeModel(string serializedList)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(HttpUtility.UrlDecode(serializedList));
    }

    public static string SerializeList(List<T> list)
    {
        var serialize = System.Text.Json.JsonSerializer.Serialize(list);
        return HttpUtility.UrlEncode(serialize);
    }

    public static string SerializeModel(T model)
    {
        var serialize = System.Text.Json.JsonSerializer.Serialize(model);
        return HttpUtility.UrlEncode(serialize);
    }
}
