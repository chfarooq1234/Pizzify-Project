using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlinePizzaWebApplication.Data;
using OnlinePizzaWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzaWebApplication.Components
{
    public class CarouselMenu : ViewComponent
    {
        private readonly ApplicationData _context;
        public CarouselMenu(ApplicationData context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pizzas =  _context.Pizzas.Where(x => x.IsPizzaOfTheWeek).ToList();
            return View(pizzas);
        }
    }
}
