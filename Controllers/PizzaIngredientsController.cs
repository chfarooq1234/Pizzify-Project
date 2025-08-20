using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlinePizzaWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using OnlinePizzaWebApplication.Data;
using OnlinePizzaWebApplication.Repositories;

namespace OnlinePizzaWebApplication.Controllers
{
    
    public class PizzaIngredientsController : Controller
    {
        
        private readonly ApplicationData _applicationData;
        private readonly IPizzaRepository _pizzaRepo;

        public PizzaIngredientsController(ApplicationData applicationData,IPizzaRepository pizzaRepository)
        {
            
            _applicationData = applicationData;
            _pizzaRepo = pizzaRepository;
        }

        // GET: PizzaIngredients
        public async Task<IActionResult> Index()
        {
            var appDbContext = _applicationData.PizzaIngredients;
            return View( appDbContext.ToList());
        }

        // GET: PizzaIngredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaIngredients = _applicationData.PizzaIngredients
                
                .SingleOrDefault(m => m.Id == id);
            if (pizzaIngredients == null)
            {
                return NotFound();
            }

            return View(pizzaIngredients);
        }

        // GET: PizzaIngredients/Create
        public IActionResult Create(int? id, [Bind("Id,PizzaId,IngredientId")] PizzaIngredients pizzaIngredients)
        {
            
            var piz = _applicationData.Pizzas.SingleOrDefault(m => m.Id == id);
            var currentIng = _applicationData.PizzaIngredients.Where(x => x.PizzaId == id).ToList();
            var newIng = _applicationData.Ingredients;
            foreach(var i in currentIng)
            {
                newIng.Remove(i.Ingredient);
            }
            List<Pizzas> pizza = new List<Pizzas> { piz };
            ViewData["IngredientId"] = new SelectList(newIng, "Id", "Name");
            ViewData["PizzaId"] = new SelectList(pizza, "Id", "Name");
            ViewData["Id"] = id;

            return View();
        }

        // POST: PizzaIngredients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PizzaId,IngredientId")] PizzaIngredients pizzaIngredients)
        {
            if (ModelState.IsValid)
            {
                var ingreden = _applicationData.Ingredients.SingleOrDefault(n => n.Id == pizzaIngredients.IngredientId);
                pizzaIngredients.Ingredient = ingreden;
                var pizza = _applicationData.Pizzas.SingleOrDefault(h => h.Id == pizzaIngredients.PizzaId);
                pizza.Price = pizza.Price + 10.00M;
                pizzaIngredients.Pizza = pizza;

                //var pizzaIngredient = _applicationData.PizzaIngredients.SingleOrDefault(m => m.Id == pizzaIngredients.IngredientId);
                _applicationData.PizzaIngredients.Add(pizzaIngredients);
                
                return RedirectToAction("DisplayDetails","Pizzas",new { id = pizzaIngredients.PizzaId });
            }
            ViewData["IngredientId"] = new SelectList(_applicationData.Ingredients, "Id", "Name", pizzaIngredients.IngredientId);
            ViewData["PizzaId"] = new SelectList(_applicationData.Pizzas, "Id", "Name", pizzaIngredients.PizzaId);
            ViewData["Id"] = pizzaIngredients.PizzaId;
            return View(pizzaIngredients);
        }

        // GET: PizzaIngredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaIngredients =  _applicationData.PizzaIngredients.SingleOrDefault(m => m.Id == id);
            if (pizzaIngredients == null)
            {
                return NotFound();
            }
            ViewData["IngredientId"] = new SelectList(_applicationData.Ingredients, "Id", "Name", pizzaIngredients.IngredientId);
            ViewData["PizzaId"] = new SelectList(_applicationData.Pizzas, "Id", "Name", pizzaIngredients.PizzaId);
            return View(pizzaIngredients);
        }

        // POST: PizzaIngredients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PizzaId,IngredientId")] PizzaIngredients pizzaIngredients)
        {
            if (id != pizzaIngredients.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaIngredientsExists(pizzaIngredients.Id))
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
            ViewData["IngredientId"] = new SelectList(_applicationData.Ingredients, "Id", "Name", pizzaIngredients.IngredientId);
            ViewData["PizzaId"] = new SelectList(_applicationData.Pizzas, "Id", "Name", pizzaIngredients.PizzaId);
            return View(pizzaIngredients);
        }

        // GET: PizzaIngredients/Delete/5
        public async Task<IActionResult> Delete(int? id,int ?ingId)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pizzas =  _pizzaRepo.GetByIdIncludedAsync(id);

            var pizzaIngredients = _applicationData.PizzaIngredients.SingleOrDefault(m => m.PizzaId == id && m.IngredientId == ingId);
            if (pizzaIngredients == null)
            {
                return NotFound();
            }
            pizzas.Price = pizzas.Price - 10.00M;
            _applicationData.PizzaIngredients.Remove(pizzaIngredients);
            return RedirectToAction("DisplayDetails", "Pizzas", new { id = id });
        }

        // POST: PizzaIngredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pizzaIngredients =  _applicationData.PizzaIngredients.SingleOrDefault((m => m.Id == id));
            _applicationData.PizzaIngredients.Remove(pizzaIngredients);
            
            return RedirectToAction("Index");
        }

        private bool PizzaIngredientsExists(int id)
        {
            return _applicationData.PizzaIngredients.Any(e => e.Id == id);
        }
    }
}
