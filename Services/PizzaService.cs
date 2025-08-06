using ContosoPizza.Models;
using ContosoPizza.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoPizza.Services;

public class PizzaService
{
    static int nextId = 0;

    private readonly PizzaRepository _repository;


    public PizzaService(PizzaRepository repository)
    {
        _repository = repository;
    }
    public List<Pizza> GetAll() => _repository.GetAllPizzaAsync().Result.ToList();

    public Pizza? Get(int id) => _repository.GetPizzaByIdAsync(id).Result;

    public async Task Add(Pizza pizza)
    {
        pizza.Id = nextId++;
        Console.WriteLine($"Adding pizza with ID: {pizza.Id}");  
        await _repository.AddPizzaAsync(pizza);
    }

    public void Delete(int id)
    {
        _repository.DeletePizzaAsync(id).Wait();
    }

    public void Update(Pizza pizza)
    {
        _repository.UpdatePizzaAsync(pizza).Wait();
    }
}