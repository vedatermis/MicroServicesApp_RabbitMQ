using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Basket.API.Data.Abstract;
using Basket.API.Entities;
using Basket.API.Repositories.Abstract;
using Newtonsoft.Json;

namespace Basket.API.Repositories.Concrete
{
    public class BasketRepository: IBasketRepository
    {
        private readonly IBasketContext _basketContext;

        public BasketRepository(IBasketContext basketContext)
        {
            _basketContext = basketContext;
        }
        
        public async Task<BasketCart> GetBasket(string userName)
        {
            var basket = await _basketContext.Redis.StringGetAsync(userName);

            if (basket.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basket)
        {
            var updated =
                await _basketContext.Redis.StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            if (!updated)
            {
                return null;
            }

            return await GetBasket(basket.UserName);
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            return await _basketContext.Redis.KeyDeleteAsync(userName);
        }
    }
}