using RGB.Back.Providers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RGB.Back.Providers
{
    public class JsonProvider
    {
        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        private JsonSerializerOptions deserializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, serializerOptions);
        }

        public T Deserialize<T>(string str)
        { return JsonSerializer.Deserialize<T>(str, deserializerOptions); 
        }
        
    }
}
