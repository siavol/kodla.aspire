using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace Kodla.Api
{
    internal class SimpleJsonKafkaSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            string json = JsonSerializer.Serialize(data);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return bytes;
        }
    }

    internal class SimpleJsonKafkaDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            string json = Encoding.UTF8.GetString(data.ToArray());
            return JsonSerializer.Deserialize<T>(json)!;
        }
    }
}