using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{

    private readonly OrderService _orderService;
    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public ActionResult<List<Order>> GetAll() =>
        _orderService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Order> Get(int id)
    {
        return _orderService.Get(id);
    }

    [HttpPost]
    public IActionResult Create(Order order)
    {
        // Console.WriteLine($"----------------------------------------------------------------------------------------------------Creating order with ID: {order}");
        _orderService.Add(order);
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Order order)
    {
        try
        {
            _orderService.Update(order);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating order: {ex.Message}");
        }
        _orderService.Update(order);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var order = _orderService.Get(id);

        if (order is null)
            return NotFound();

        _orderService.Delete(id);

        return NoContent();
    }
    
    
}