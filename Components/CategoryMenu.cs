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
    public class CategoryMenu : ViewComponent
    {
        
        private readonly ApplicationData _applicationdata;
        public CategoryMenu(ApplicationData applicationData)
        {
            
            _applicationdata = applicationData;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories =  _applicationdata.Categories.OrderBy(c => c.Name).ToList();
            return View(categories);
        }
    }
}
