using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("user/[controller]")]
public class AdminController : ControllerBase
{

    private readonly UserService _userService;
    private readonly OrderService _orderService;
    private readonly PizzaService _pizzaService;
    public AdminController(UserService userService, OrderService orderService, PizzaService pizzaService)
    {
        _userService = userService;
        _orderService = orderService;
        _pizzaService = pizzaService;
    }




    [HttpGet]
    public ActionResult<AdminHomeData> GetAll() =>
        new AdminHomeData
        {
            Users = _userService.GetAll(),
            Orders = _orderService.GetAll(),
            Pizzas = _pizzaService.GetAll()
        };

    [HttpGet("allorders")]
    public async Task<ActionResult<GetOrdersResponse>> ViewOrders(int pageNumber, int pageSize, bool asc, bool byprice)
    {
        var orders = await _orderService.GetOrdersAsync(pageNumber, pageSize, asc, byprice);
        var totalPages = await _orderService.GetTotalPages(pageSize, asc, byprice);
        Console.WriteLine($"Total Pages: {totalPages}");
        return new GetOrdersResponse { Orders = orders.ToList(), TotalPages = totalPages };
    }

    [HttpPost("addorder")]
    public async Task<IActionResult> Create(Order order)
    {
        // Console.WriteLine($"----------------------------------------------------------------------------------------------------Creating order with ID: {order}");
        await _orderService.Add(order);
        return CreatedAtAction(order.Id.ToString(), order);
    }
    
    
}