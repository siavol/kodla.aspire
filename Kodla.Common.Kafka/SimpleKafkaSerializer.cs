using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace Kodla.Common.Kafka;

public class SimpleJsonKafkaSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        string json = JsonSerializer.Serialize(data);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }
}

public class SimpleJsonKafkaDeserializer<T> : IDeserializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        string json = Encoding.UTF8.GetString(data.ToArray());
        return JsonSerializer.Deserialize<T>(json)!;
    }
}
