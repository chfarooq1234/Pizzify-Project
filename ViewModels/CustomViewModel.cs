using OnlinePizzaWebApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzaWebApplication.ViewModels
{
    public class CustomViewModel
    {
        public IEnumerable<Pizzas> pizaa { get; set; }
        public IEnumerable<PizzaIngredients> PizzaIngredients { get; set; }
        public List<Toppings> toppings { get; set; }
        public List<Ingredients> Ingredients { get; set; }
        [DisplayName("Select Pizza")]
        public int PizzaId { get; set; }

        [DisplayName("Select Ingredient")]
        public int IngredientId { get; set; }

        [DisplayName("Select Category")]
        public int CategoriesId { get; set; }

        [Range(0, 1000)]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }
    }
}
