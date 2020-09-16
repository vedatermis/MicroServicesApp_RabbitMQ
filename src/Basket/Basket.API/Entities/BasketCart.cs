using System.Collections.Generic;

namespace Basket.API.Entities
{
    public class BasketCart
    {
        public BasketCart()
        {
            
        }

        public BasketCart(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }
        public List<BasketCartItem> BasketCartItems { get; set; } = new List<BasketCartItem>();

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;

                foreach (var basketCartItem in BasketCartItems)
                {
                    totalPrice += basketCartItem.Price * basketCartItem.Quantity;
                }

                return totalPrice;
            }
        }
    }
}