using System.Text.Json;

namespace CaveGame;

public class Deserializer
{
	public static Dictionary<string, T> DeserializeJson<T>(string fileLocation)
	{
		var stream = File.OpenRead(fileLocation);
		var deserialized = JsonSerializer.Deserialize<Dictionary<string, T>>(stream);
		return deserialized;
	}
}