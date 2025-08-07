using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    private readonly UserService _userService;
    public LoginController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<List<Order>> GetAll() =>
        new List<Order>();

    [HttpPost]
    public async Task<ActionResult<User>> Login([FromBody] User user)
    {
        var loggedInUser = await _userService.LoginUserAsync(user.UserName, user.Passcode);
        if (loggedInUser == null)
        {
            return Unauthorized("Invalid username or passcode.");
        }
        return Ok(loggedInUser);
    }

    
    
}