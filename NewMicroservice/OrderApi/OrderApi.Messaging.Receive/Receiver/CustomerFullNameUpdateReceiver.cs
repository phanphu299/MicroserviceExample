
namespace OrderApi.Messaging.Receive.Receiver
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using OrderApi.Domains.Entities;
    using OrderApi.Messaging.Receive.Message;
    using OrderApi.Repositories;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class CustomerFullNameUpdateReceiver : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        private readonly IServiceScopeFactory _scopeFactory;

        public CustomerFullNameUpdateReceiver(IServiceScopeFactory scopeFactory)
        {
            _hostname = "127.0.0.1";
            _queueName = "CustomerQueue";
            _username = "guest";
            _password = "guest";
            _scopeFactory = scopeFactory;

            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);

                var updateCustomerFullNameModel = JsonConvert.DeserializeObject<UpdateCustomerMessage>(message);

                HandleMessage(updateCustomerFullNameModel);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async void HandleMessage(UpdateCustomerMessage model)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    var _repository = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();

                    var order = await _repository
                            .GetAll()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.CustomerGuid == model.Id);

                    if (order == null)
                        return;

                    order.CustomerFullName = $"{model.FirstName} {model.LastName}";
                    await _repository.UpdateAsync(order);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
