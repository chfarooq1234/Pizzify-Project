using Microsoft.EntityFrameworkCore;
using OnlinePizzaWebApplication.Data;
using OnlinePizzaWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzaWebApplication.Repositories
{
    public class PizzaRepository : IPizzaRepository
    {
        
        private ApplicationData _applicationData;

        public PizzaRepository(ApplicationData applicationData)
        {
            
            _applicationData = applicationData;
        }

        public IEnumerable<Pizzas> Pizzas => _applicationData.Pizzas; //include here

        public IEnumerable<Pizzas> PizzasOfTheWeek => _applicationData.Pizzas.Where(p => p.IsPizzaOfTheWeek);

        public void Add(Pizzas pizza)
        {
            _applicationData.Pizzas.Add(pizza);
        }

        public IEnumerable<Pizzas> GetAll()
        {
            return _applicationData.Pizzas.ToList();
        }

        public async Task<IEnumerable<Pizzas>> GetAllAsync()
        {
            return  _applicationData.Pizzas.ToList();
        }

        public async Task<IEnumerable<Pizzas>> GetAllIncludedAsync()
        {
            return  _applicationData.Pizzas.ToList();
        }

        public IEnumerable<Pizzas> GetAllIncluded()
        {
            return _applicationData.Pizzas.ToList();
        }

        public Pizzas GetById(int? id)
        {
            return _applicationData.Pizzas.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Pizzas> GetByIdAsync(int? id)
        {
            return  _applicationData.Pizzas.FirstOrDefault(p => p.Id == id);
        }

        public Pizzas GetByIdIncluded(int? id)
        {
            return _applicationData.Pizzas.FirstOrDefault(p => p.Id == id);
        }

        public Pizzas GetByIdIncludedAsync(int? id)
        {
            return _applicationData.Pizzas.SingleOrDefault(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _applicationData.Pizzas.Any(p => p.Id == id);
        }

        public void Remove(Pizzas pizza)
        {
            _applicationData.Pizzas.Remove(pizza);
        }

        public async Task SaveChangesAsync()
        {
            
        }

        public void SaveChanges()
        {
            
        }

        public void Update(Pizzas pizza)
        {
            
        }

    }
}
