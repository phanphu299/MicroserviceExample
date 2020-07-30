
namespace CustomerApi.Message.Send.Sender
{
    using CustomerApi.Message.Send.Message;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using System.Text;

    public class CustomerUpdateSender : ICustomerUpdateSender
    {
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public CustomerUpdateSender(IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _hostname = "127.0.0.1";
            _queueName = "CustomerQueue";
            _username = "guest";
            _password = "guest";
        }

        public void SendUpdateCustomer(UpdateCustomerMessage customer)
        {
            var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonConvert.SerializeObject(customer);
                var body = Encoding.UTF8.GetBytes(json);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: properties, body: body);
            }
        }
    }
}
