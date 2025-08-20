using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlinePizzaWebApplication.Models;
using OnlinePizzaWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using OnlinePizzaWebApplication.ViewModels;
using Newtonsoft.Json;
using OnlinePizzaWebApplication.Data;

namespace OnlinePizzaWebApplication.Controllers
{
    
    public class PizzasController : Controller
    {
        
        private readonly IPizzaRepository _pizzaRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ApplicationData _applicationData;

        public PizzasController( IPizzaRepository pizzaRepo, ICategoryRepository categoryRepo,ApplicationData applicationData,IPizzaIngredientsRepository pizzaIngredientsRepository)
        {
            
            _applicationData = applicationData;
            _pizzaRepo = pizzaRepo;
            _categoryRepo = categoryRepo;
            
        }

        // GET: Pizzas
        public async Task<IActionResult> Index()
        {
            return View(await _pizzaRepo.GetAllIncludedAsync());
        }

        // GET: Pizzas
        [AllowAnonymous]
        public async Task<IActionResult> ListAll()
        {
            var model = new SearchPizzasViewModel()
            {
                PizzaList = await _pizzaRepo.GetAllIncludedAsync(),
                SearchText = null
            };

            return View(model);
        }

        private async Task<List<Pizzas>> GetPizzaSearchList(string userInput)
        {
            userInput = userInput.ToLower().Trim();

            var result = _applicationData.Pizzas
                .Where(p => p
                    .Name.ToLower().Contains(userInput))
                    .Select(p => p).OrderBy(p => p.Name);

            return  result.ToList();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AjaxSearchList(string searchString)
        {
            var pizzaList = await GetPizzaSearchList(searchString);
            
            return PartialView(pizzaList);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListAll([Bind("SearchText")] SearchPizzasViewModel model)
        {
            var pizzas = await _pizzaRepo.GetAllIncludedAsync();
            if (model.SearchText == null || model.SearchText == string.Empty)
            {
                model.PizzaList = pizzas;
                return View(model);
            }

            var input = model.SearchText.Trim();
            if (input == string.Empty || input == null)
            {
                model.PizzaList = pizzas;
                return View(model);
            }
            var searchString = input.ToLower();

            if (string.IsNullOrEmpty(searchString))
            {
                model.PizzaList = pizzas;
            }
            else
            {
                var pizzaList = _applicationData.Pizzas.OrderBy(x => x.Name)
                     .Where(p =>
                     p.Name.ToLower().Contains(searchString)
                  || p.Price.ToString("c").ToLower().Contains(searchString)
                  || p.Category.Name.ToLower().Contains(searchString)
                  || p.PizzaIngredients.Select(x => x.Ingredient.Name.ToLower()).Contains(searchString))
                    .ToList();

                if (pizzaList.Any())
                {
                    model.PizzaList = pizzaList;
                }
                else
                {
                    model.PizzaList = new List<Pizzas>();
                }

            }
            return View(model);
        }

        // GET: Pizzas
        [AllowAnonymous]
        public async Task<IActionResult> ListCategory(string categoryName)
        {
            bool categoryExtist = _applicationData.Categories.Any(c => c.Name == categoryName);
            if (!categoryExtist)
            {
                return NotFound();
            }

            var category =  _applicationData.Categories.FirstOrDefault(c => c.Name == categoryName);

            if(categoryName == "Custom Pizza")
            {
         
                return RedirectToAction("Create","Pizzas");
            }
            if (category == null)
            {
                return NotFound();
            }

            bool anyPizzas = _applicationData.Pizzas.Any(x => x.Category == category);
            if (!anyPizzas)
            {
                return NotFound($"No Pizzas were found in the category: {categoryName}");
            }

            var pizzas = _applicationData.Pizzas.Where(x => x.Category == category);
                

            ViewBag.CurrentCategory = category.Name;
            return View(pizzas);
        }

        // GET: Pizzas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzas =  _pizzaRepo.GetByIdIncludedAsync(id);

            if (pizzas == null)
            {
                return NotFound();
            }

            return View(pizzas);
        }

        // GET: Pizzas/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> DisplayDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var pizzas =  _pizzaRepo.GetByIdIncludedAsync(id);

            var listOfIngredients = _applicationData.PizzaIngredients.Where(p => p.PizzaId == id).ToList();
            ViewBag.PizzaIngredients = listOfIngredients;
            ViewBag.PizzaId = id;

            //var listOfReviews = await _context.Reviews.Where(x => x.PizzaId == id).Select(x => x).ToListAsync();
            //ViewBag.Reviews = listOfReviews;
            double score = 0;
            
            ViewBag.AverageReviewScore = score;

            if (pizzas == null)
            {
                return NotFound();
            }

            return View(pizzas);
        }

        // GET: Pizzas
        [AllowAnonymous]
        public async Task<IActionResult> SearchPizzas()
        {
            var model = new SearchPizzasViewModel()
            {
                PizzaList = await _pizzaRepo.GetAllIncludedAsync(),
                SearchText = null
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchPizzas([Bind("SearchText")] SearchPizzasViewModel model)
        {
            var pizzas = await _pizzaRepo.GetAllIncludedAsync();
            var search = model.SearchText.ToLower();

            if (string.IsNullOrEmpty(search))
            {
                model.PizzaList = pizzas;
            }
            else
            {
                var pizzaList = _applicationData.Pizzas.OrderBy(x => x.Name)
                    .Where(p =>
                     p.Name.ToLower().Contains(search)
                  || p.Price.ToString("c").ToLower().Contains(search)
                  || p.Category.Name.ToLower().Contains(search)
                  || p.PizzaIngredients.Select(x => x.Ingredient.Name.ToLower()).Contains(search)).ToList();

                if (pizzaList.Any())
                {
                    model.PizzaList = pizzaList;
                }
                else
                {
                    model.PizzaList = new List<Pizzas>();
                }

            }
            return View(model);
        }

        // GET: Pizzas/
        [AllowAnonymous]
        public IActionResult Create()
        {
            

                CustomViewModel customView = new CustomViewModel();
            var ingredientprice = _applicationData.Ingredients[0].price;
            var toppingsprice = _applicationData.Toppings[0].price;
            decimal price = ingredientprice + toppingsprice;
                customView.Price = price;

                ViewBag.IngredientId = new SelectList(_applicationData.Ingredients, "Id", "Name");
                ViewBag.CategoriesId = new SelectList(_applicationData.Toppings, "Id", "Name");
                return View("CustomPizaa", customView);
            

            

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult UpdatePrice(string categoryId = null, string IngredientId = null)
        {
            ModelState.Clear();
            CustomViewModel customView = new CustomViewModel();
            
            decimal price = 0.00M;
            
            if (!string.IsNullOrEmpty(categoryId) && !string.IsNullOrEmpty(IngredientId))
            {
                var Ingred = Convert.ToInt32(IngredientId);
                var cate = Convert.ToInt32(categoryId);
                var ingredients = _applicationData.Ingredients.SingleOrDefault(m => m.Id == Ingred);
                var toppings = _applicationData.Toppings.SingleOrDefault(n => n.Id == cate);

                price = ingredients.price + toppings.price;
            }
            
            
            string p = price.ToString("c");
            return Ok(p);
            //return RedirectToAction("Create",new { Ingredients = customView.Ingredients,toppings = customView.toppings,Price = customView.Price });
        }
        // POST: Pizzas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,ImageUrl,IsPizzaOfTheWeek,CategoriesId")] Pizzas pizzas)
        {
            if (ModelState.IsValid)
            {
                _pizzaRepo.Add(pizzas);
                await _pizzaRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", pizzas.CategoriesId);
            return View(pizzas);
        }

        // GET: Pizzas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzas = await _pizzaRepo.GetByIdAsync(id);

            if (pizzas == null)
            {
                return NotFound();
            }
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", pizzas.CategoriesId);
            return View(pizzas);
        }

        // POST: Pizzas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,ImageUrl,IsPizzaOfTheWeek,CategoriesId")] Pizzas pizzas)
        {
            if (id != pizzas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _pizzaRepo.Update(pizzas);
                    await _pizzaRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzasExists(pizzas.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CategoriesId"] = new SelectList(_categoryRepo.GetAll(), "Id", "Name", pizzas.CategoriesId);
            return View(pizzas);
        }

        // GET: Pizzas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzas = _pizzaRepo.GetByIdIncludedAsync(id);

            if (pizzas == null)
            {
                return NotFound();
            }

            return View(pizzas);
        }

        // POST: Pizzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pizzas = await _pizzaRepo.GetByIdAsync(id);
            _pizzaRepo.Remove(pizzas);
            await _pizzaRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        private bool PizzasExists(int id)
        {
            return _pizzaRepo.Exists(id);
        }
    }
}
