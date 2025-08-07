using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly UserService _userService;
    private readonly OrderService _orderService;
    private readonly PizzaService _pizzaService;
    public UserController(UserService userService, OrderService orderService, PizzaService pizzaService)
    {
        _userService = userService;
        _orderService = orderService;
        _pizzaService = pizzaService;
    }

    [HttpGet("{userName}")]
    public async Task<IEnumerable<Order>> GetUserOrders(string userName)
    {
        
        var orders = await _orderService.GetOrdersByUserName(userName);
        return orders.ToList();
    }

    [HttpPut()]
    public IActionResult Update([FromBody] User user)
    {
        if (user == null || user.Id <= 0)
        {
            return BadRequest("Invalid user data.");
        }

        var existingUser = _userService.Get(user.Id);
        if (existingUser == null)
        {
            return NotFound("User not found.");
        }

        existingUser.UserName = user.UserName;
        existingUser.Passcode = user.Passcode;
        existingUser.UserType = user.UserType; // Assuming UserType is a field to update

        _userService.Update(existingUser);

        return NoContent();
    }
    

    [HttpDelete("{username}")]
    public IActionResult Delete(string username)
    {
        var user = _userService.GetUserByUserName(username);

        if (user is null)
            return NotFound();

        _userService.Delete(username);

        return NoContent();
    }


}