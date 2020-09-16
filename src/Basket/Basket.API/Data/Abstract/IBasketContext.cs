using StackExchange.Redis;

namespace Basket.API.Data.Abstract
{
    public interface IBasketContext
    {
        IDatabase Redis { get; }
    }
}