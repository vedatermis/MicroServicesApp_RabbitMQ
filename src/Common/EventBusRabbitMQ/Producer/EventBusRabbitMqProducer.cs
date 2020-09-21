using System;
using System.Text;
using EventBusRabbitMQ.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EventBusRabbitMQ.Producer
{
    public class EventBusRabbitMqProducer
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;

        public EventBusRabbitMqProducer(IRabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void PublishBasketCheckout(string queueName, BasketCheckoutEvent publishModel)
        {
            using (var channel = _rabbitMqConnection.CreateModel())
            {
                channel.QueueDeclare(queueName, false, false, false, null);
                var message = JsonConvert.SerializeObject(publishModel);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish("", queueName, true, properties, body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent RabbitMQ");
                };

                channel.ConfirmSelect();
            }
        }
    }
}