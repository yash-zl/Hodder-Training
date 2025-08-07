using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{

    private readonly UserService _userService;
    public RegisterController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Register([FromBody] User user)
    {
        var AlreadyRegisteredUser = await _userService.GetUserByUserName(user.UserName);
        if (AlreadyRegisteredUser == null)
        {
            _userService.RegisterUser(user);
            return Ok();
        }

        return Unauthorized("Username taken. Please choose another username.");
    }
}


