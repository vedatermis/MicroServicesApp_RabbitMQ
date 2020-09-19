using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositories.Abstract
{
    public interface IBasketRepository
    {
        Task<BasketCart> GetBasket(string userName);
        Task<BasketCart> UpdateBasket(BasketCart basket);
        Task<bool> DeleteBasket(string userName);
    }
}