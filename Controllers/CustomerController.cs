using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{

    private readonly CustomerService _customerService;
    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public ActionResult<List<Customer>> GetAll() =>
        _customerService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Customer> Get(int id)
    {
        return _customerService.Get(id);
    }

    [HttpPost]
    public IActionResult Create(Customer customer)
    {
        // Console.WriteLine($"----------------------------------------------------------------------------------------------------Creating customer with ID: {customer}");
        _customerService.Add(customer);
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Customer customer)
    {
        try
        {
            _customerService.Update(customer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating customer: {ex.Message}");
        }
        _customerService.Update(customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var customer = _customerService.Get(id);

        if (customer is null)
            return NotFound();

        _customerService.Delete(id);

        return NoContent();
    }
    
    
}