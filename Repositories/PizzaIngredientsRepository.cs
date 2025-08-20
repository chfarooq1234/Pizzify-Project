using Microsoft.EntityFrameworkCore;
using OnlinePizzaWebApplication.Data;
using OnlinePizzaWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzaWebApplication.Repositories
{
    public class PizzaIngredientsRepository : IPizzaIngredientsRepository
    {
        private readonly ApplicationData _context;
        

        public PizzaIngredientsRepository(ApplicationData context)
        {
            _context = context;
            
        }

        public IEnumerable<PizzaIngredients> PizzaIngredients => _context.PizzaIngredients; //include here

        public void Add(PizzaIngredients pizzaIngredient)
        {
            _context.PizzaIngredients.Add(pizzaIngredient);
        }

        public IEnumerable<PizzaIngredients> GetAll()
        {
            return _context.PizzaIngredients.ToList();
        }

        public IEnumerable<PizzaIngredients> GetByPizzaId(int ? id)
        {
            return _context.PizzaIngredients.Where(x => x.PizzaId == id).ToList();
        }

        public async Task<IEnumerable<PizzaIngredients>> GetAllAsync()
        {
            return _context.PizzaIngredients.ToList();
        }

        public PizzaIngredients GetById(int? id)
        {
            return _context.PizzaIngredients.FirstOrDefault(p => p.Id == id);
        }

        public async Task<PizzaIngredients> GetByIdAsync(int? id)
        {
            return _context.PizzaIngredients.FirstOrDefault(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.PizzaIngredients.Any(p => p.Id == id);
        }

        public void Remove(PizzaIngredients pizzaIngredient)
        {
            _context.PizzaIngredients.Remove(pizzaIngredient);
        }

        public void SaveChanges()
        {
           
        }

        public async Task SaveChangesAsync()
        {
            
        }

        public void Update(PizzaIngredients pizzaIngredient)
        {
            
        }
    }
}
