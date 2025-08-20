using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlinePizzaWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using OnlinePizzaWebApplication.Data;

namespace OnlinePizzaWebApplication.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationData _context;

        public CategoryRepository(ApplicationData context)
        {
            _context = context;
        }

        public IEnumerable<Categories> Categories => _context.Categories; //include here

        public void Add(Categories category)
        {
            _context.Categories.Add(category);
        }

        public IEnumerable<Categories> GetAll()
        {
            return _context.Categories.ToList();
        }

        public async Task<IEnumerable<Categories>> GetAllAsync()
        {
            return  _context.Categories.ToList();
        }

        public Categories GetById(int? id)
        {
            return _context.Categories.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Categories> GetByIdAsync(int? id)
        {
            return _context.Categories.FirstOrDefault(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Pizzas.Any(p => p.Id == id);
        }

        public void Remove(Categories category)
        {
            _context.Categories.Remove(category);
        }

        public void SaveChanges()
        {
            
        }

        public async Task SaveChangesAsync()
        {
            
        }

        public void Update(Categories category)
        {
            
        }

    }
}
