using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlinePizzaWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using OnlinePizzaWebApplication.Data;

namespace OnlinePizzaWebApplication.Repositories
{
    public class IngredientsRepository : IIngredientsRepository
    {
        private readonly ApplicationData _context;

        public IngredientsRepository(ApplicationData context)
        {
            _context = context;
        }

        public IEnumerable<Ingredients> Ingredients => _context.Ingredients; //include here

        public void Add(Ingredients ingredient)
        {
            _context.Ingredients.Add(ingredient);
        }

        public IEnumerable<Ingredients> GetAll()
        {
            return _context.Ingredients.ToList();
        }

        public async Task<IEnumerable<Ingredients>> GetAllAsync()
        {
            return _context.Ingredients.ToList();
        }

        public Ingredients GetById(int? id)
        {
            return _context.Ingredients.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Ingredients> GetByIdAsync(int? id)
        {
            return _context.Ingredients.FirstOrDefault(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Ingredients.Any(p => p.Id == id);
        }

        public void Remove(Ingredients ingredient)
        {
            _context.Ingredients.Remove(ingredient);
        }

        public void Update(Ingredients ingredient)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
