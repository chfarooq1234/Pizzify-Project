using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlinePizzaWebApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzaWebApplication.Models
{
    public class ShoppingCart
    {
        private readonly ApplicationData _applicationData;

        private ShoppingCart(ApplicationData applicationData)
        {
           _applicationData = applicationData;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }


        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<ApplicationData>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public async Task AddToCartAsync(Pizzas pizza, int amount)
        {
            var shoppingCartItem =
                    _applicationData.ShoppingCartItems.SingleOrDefault(
                        s => s.Pizza.Id == pizza.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pizza = pizza,
                    Amount = 1
                };

                _applicationData.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            
        }

        public async Task<int> RemoveFromCartAsync(int id)
        {
            var shoppingCartItem =
                     _applicationData.ShoppingCartItems.SingleOrDefault(
                        s => s.Pizza.Id == id && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _applicationData.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            

            return localAmount;
        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartItemsAsync()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems = 
                       _applicationData.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .ToList());
        }

        public async Task ClearCartAsync()
        {
            var cartItems = _applicationData
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);
            foreach (var d in cartItems.ToList())
            {
                _applicationData.ShoppingCartItems.Remove(d);
                    }

            
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _applicationData.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Pizza.Price * c.Amount).Sum();
            return total;
        }

    }
}
