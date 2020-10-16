using System.Text;
using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands;
using Ordering.Core.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ordering.API.RabbitMq
{
    public class EventBusRabbitMqConsumer
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public EventBusRabbitMqConsumer(IOrderRepository orderRepository, IMapper mapper, IMediator mediator, IRabbitMqConnection rabbitMqConnection)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _mediator = mediator;
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void Consume()
        {
            var channel = _rabbitMqConnection.CreateModel();
            channel.QueueDeclare(EventBusConstants.BasketCheckoutQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(queue: EventBusConstants.BasketCheckoutQueue, autoAck: true, consumer: consumer);
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EventBusConstants.BasketCheckoutQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);

                var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);
                var result = await _mediator.Send(command);
            }
        }

        public void Disconnect()
        {
            _rabbitMqConnection.Dispose();
        }
    }
}