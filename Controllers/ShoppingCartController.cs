using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlinePizzaWebApplication.Repositories;
using OnlinePizzaWebApplication.Models;
using OnlinePizzaWebApplication.ViewModels;
using OnlinePizzaWebApplication.Data;
using Microsoft.AspNetCore.Authorization;

namespace OnlinePizzaWebApplication.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly ApplicationData _applicationData;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IPizzaRepository pizzaRepository,
            ShoppingCart shoppingCart, ApplicationData applicationData)
        {
            _pizzaRepository = pizzaRepository;
            _shoppingCart = shoppingCart;
            _applicationData = applicationData;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var items = await _shoppingCart.GetShoppingCartItemsAsync();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }

        public async Task<IActionResult> AddToShoppingCart(int pizzaId)
        {
            var selectedPizza = await _pizzaRepository.GetByIdAsync(pizzaId);

            if (selectedPizza != null)
            {
                await _shoppingCart.AddToCartAsync(selectedPizza, 1);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CustomAddToShoppingCart(string ingredeientId,string toppingsId,string price)
        {
            Pizzas pizzas = new Pizzas();
            var p = price.Replace("$", "");
            var newPrice = Convert.ToDecimal(p);
            var ings = Convert.ToInt32(ingredeientId);
            var tpgs = Convert.ToInt32(toppingsId);
            var ing = _applicationData.Ingredients.SingleOrDefault(m => m.Id == ings);
            var top = _applicationData.Toppings.SingleOrDefault(n => n.Id == tpgs);
            List<PizzaIngredients> png = new List<PizzaIngredients> { new PizzaIngredients { Ingredient = ing} };

            pizzas.PizzaIngredients = png;
            Random random = new Random();
            pizzas.Id = random.Next();
            pizzas.Name = "Custom Pizza";
            pizzas.IsPizzaOfTheWeek = false;
            pizzas.Price = newPrice;
           
            
            await _shoppingCart.AddToCartAsync(pizzas, 1);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromShoppingCart(int pizzaId)
        {
            
                await _shoppingCart.RemoveFromCartAsync(pizzaId);
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCart()
        {
            await _shoppingCart.ClearCartAsync();

            return RedirectToAction("Index");
        }

    }
}