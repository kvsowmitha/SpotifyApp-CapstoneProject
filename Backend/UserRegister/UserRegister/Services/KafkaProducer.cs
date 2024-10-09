using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace UserRegister.Services
{
    public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private readonly KafkaSettings _kafkaSettings;

        public KafkaProducer(IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaSettings = kafkaSettings.Value;
        }

        public async Task<bool> ProduceAsync(string topic, Message<TKey, TValue> message)
        {
            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = _kafkaSettings.BootstrapServers
                };

                using (var producer = new ProducerBuilder<TKey, TValue>(config).Build())
                {
                    var deliveryReport = await producer.ProduceAsync(topic, message);
                    return deliveryReport.Status == PersistenceStatus.Persisted;
                }
            }
            catch (Exception ex)
            {
                // Log error or handle it accordingly
                Console.WriteLine($"Kafka error: {ex.Message}");
                return false;
            }
        }
    }
}
