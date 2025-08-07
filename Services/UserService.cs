using ContosoPizza.Models;
using ContosoPizza.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoPizza.Services;

public class UserService
{
    static int nextId = 0;

    private readonly UserRepository _repository;


    public UserService(UserRepository repository)
    {
        _repository = repository;
    }
    public List<User> GetAll() => _repository.GetAllUsersAsync().Result.ToList();

    public User? Get(int id) => _repository.GetUserByIdAsync(id).Result;
    
    public async Task<User?> LoginUserAsync(string username, string passcode)
    {
        var user = await _repository.GetUserByCredentialsAsync(username, passcode);
        return user;
    }

    public async Task<User?> GetUserByUserName(string userName)
    {
        return await _repository.GetUserByUserNameAsync(userName);
    }

    public User? RegisterUser(User user)
    {
        // Console.WriteLine($"Registering user: {user.UserName}, {user.Passcode}, {user.UserType}");
        _repository.AddUserAsync(user).Wait();
        return user;
    }

    public void Delete(string username)
    {
        _repository.DeleteUserAsync(username).Wait();
    }

    public void Update(User user)
    {
        _repository.UpdateUserAsync(user).Wait();
    }
}