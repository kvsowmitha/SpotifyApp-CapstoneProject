using System.Text.Json;
using AuthenticationService.Model;
using AuthenticationService.Repository;
using Confluent.Kafka;

namespace AuthenticationService.Services
{
    public class ConsumerService : IHostedService
    {
        private readonly string topic = "team5";
        private readonly string groupId = "consumer-group1";
        private readonly string bootstrapServers = "localhost:9092";
        private readonly IServiceScopeFactory _scopeFactory;
        private Thread thread;
        private CancellationTokenSource tSource = new CancellationTokenSource();

        public ConsumerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            thread = new Thread(Listen);
            thread.Start();
            return Task.CompletedTask;
        }

        private void Listen()
        {
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(topic);
                try
                {
                    Console.WriteLine("Kafka consumer started listening....");
                    while (!tSource.Token.IsCancellationRequested)
                    {
                        var consumer = consumerBuilder.Consume();
                        var signRequest = JsonSerializer.Deserialize<RecieveUser>(consumer.Message.Value);
                        Console.WriteLine($"Processing Login Id: {signRequest.Email} {signRequest.Password}");

                        var addobj = new LoginData
                        {
                            Email = signRequest.Email,
                            Password = signRequest.Password
                        };

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var authenticationService = scope.ServiceProvider.GetRequiredService<AuthRepo>();
                            authenticationService.AddUserData(addobj);
                        }
                    }
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine(ex.Error.Reason);
                }
                finally
                {
                    consumerBuilder.Close();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            tSource.Cancel();
            return Task.CompletedTask;
        }
    }
}
