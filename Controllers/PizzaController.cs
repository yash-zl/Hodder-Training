using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{

    private readonly PizzaService _pizzaService;
    public PizzaController(PizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }

    [HttpGet]
    public ActionResult<List<Pizza>> GetAll() =>
        _pizzaService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        return _pizzaService.Get(id);
    }

    [HttpPost]
    public IActionResult Create(Pizza pizza)
    {
        // Console.WriteLine($"----------------------------------------------------------------------------------------------------Creating pizza with ID: {pizza}");
        _pizzaService.Add(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Pizza pizza)
    {
        try
        {
            _pizzaService.Update(pizza);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating pizza: {ex.Message}");
        }
        _pizzaService.Update(pizza);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = _pizzaService.Get(id);

        if (pizza is null)
            return NotFound();

        _pizzaService.Delete(id);

        return NoContent();
    }
    
    
}