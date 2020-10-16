﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Mapper;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderHandler: IRequestHandler<CheckoutOrderCommand, OrderResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public CheckoutOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
           // var orderEntity = OrderMapper.Mapper.Map<Order>(request);

            var orderEntity = new Order
            {
                AddressLine =  request.AddressLine,
                CVV = request.CVV,
                CardName = request.CardName,
                CardNumber = request.CardNumber,
                Country = request.Country,
                EmailAddress = request.EmailAddress,
                Expiration = request.Expiration,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PaymentMethod = request.PaymentMethod,
                State = request.State,
                TotalPrice = request.TotalPrice,
                UserName = request.UserName,
                ZipCode = request.ZipCode
            };

            if (orderEntity == null)
            {
                throw new ApplicationException("Not Mapped");
            }

            var newOrder = await _orderRepository.AddAsync(orderEntity);
            var orderResponse = OrderMapper.Mapper.Map<OrderResponse>(newOrder);

            return orderResponse;

        }
    }
}