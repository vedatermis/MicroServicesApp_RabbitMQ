using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryAvailability = retry.Value;

            try
            {
                orderContext.Database.Migrate();

                if (!orderContext.Orders.Any())
                {
                    orderContext.Orders.AddRange(GetPreconfiguredOrders());
                    await orderContext.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                if (retryAvailability < 3)
                {
                    retryAvailability++;
                    var log = loggerFactory.CreateLogger<OrderContextSeed>();
                    log.LogError(exception.Message);

                    await SeedAsync(orderContext, loggerFactory, retryAvailability);
                }
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order()
                {
                    UserName = "vedatermis",
                    FirstName = "Vedat",
                    LastName = "ERMİŞ",
                    EmailAddress = "vedatermis@hotmail.com",
                    AddressLine = "Beylikdüzü",
                    Country = "Turkey"
                }
            };
        }
    }
}