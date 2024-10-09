using Confluent.Kafka;
using System.Threading.Tasks;

namespace UserRegister.Services
{
    public interface IKafkaProducer<TKey, TValue>
    {
        Task<bool> ProduceAsync(string topic, Message<TKey, TValue> message);
    }
}
