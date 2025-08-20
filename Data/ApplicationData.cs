using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlinePizzaWebApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace OnlinePizzaWebApplication.Data
{
    public class ApplicationData
    {
        List<Categories> Categorie = new List<Categories>();
        List<Pizzas> Pizza = new List<Pizzas>();
        List<Ingredients> Ingredient = new List<Ingredients>();
        List<PizzaIngredients> PizzaIngredient = new List<PizzaIngredients>();
        
        List<ShoppingCartItem> ShoppingCartItem = new List<ShoppingCartItem>();
            List<Order> Order = new List<Order>();
        List<OrderDetail> OrderDetail = new List<OrderDetail>();
        List<Toppings> Topping = new List<Toppings>();

        public ApplicationData()
        {

        }

        public List<Categories> Categories { get { return Categorie; } set { Categorie = value; } }
        public List<Pizzas> Pizzas { get { return Pizza; } set { Pizza = value; } }
        public List<Ingredients> Ingredients { get { return Ingredient; } set { Ingredient = value; } }
        public List<PizzaIngredients> PizzaIngredients { get { return PizzaIngredient; } set { PizzaIngredient = value; } }
        public List<ShoppingCartItem> ShoppingCartItems { get { return ShoppingCartItem; } set { ShoppingCartItem = value; } }
        public List<Order> Orders { get { return Order; } set { Order = value; } }
        public List<OrderDetail> OrderDetails { get { return OrderDetail; } set { OrderDetail = value; } }
        public List<Toppings> Toppings { get { return Topping; } set { Topping = value; } }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //      "Server = (localdb)\\mssqllocaldb; Database = PizzaShop; Trusted_Connection = True; ");
        //}

    }
}
