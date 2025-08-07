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
    public async Task<ActionResult<List<Order>>> ViewOrders(int pageNumber = 1, int pageSize = 10, bool asc = true, bool byprice = true)
    {
        var orders = await _orderService.GetOrdersAsync(pageNumber, pageSize, asc, byprice);
        return orders.ToList();
    }
    
    
    
}