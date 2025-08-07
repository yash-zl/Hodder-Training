using ContosoPizza.Data;
using ContosoPizza.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;


namespace ContosoPizza.Repositories;

public class UserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var query = "SELECT * FROM Users";

        using (var connection = _context.CreateConnection())
        {
            var Users = await connection.QueryAsync<User>(query);
            return Users;
        }
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        var query = "SELECT * FROM Users WHERE UserName = @UserName";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { UserName = userName });
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var query = "SELECT * FROM Users WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
        }
    }

    public async Task AddUserAsync(User user)
    {
        var query = "AddUser";
        // Console.WriteLine($"Registering user: {user.UserName}, {user.Passcode}, {user.UserType}");

        var parameters = new DynamicParameters();
        parameters.Add("username", user.UserName);
        parameters.Add("passcode", user.Passcode);
        parameters.Add("usertype", user.UserType);

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task UpdateUserAsync(User User)
    {
        var query = "UpdateUser";
        var parameters = new DynamicParameters();
        parameters.Add("userid", User.Id);
        parameters.Add("username", User.UserName);
        parameters.Add("passcode", User.Passcode);

        using (var connection = _context.CreateConnection())
        {
            Object res = null;
            try
            {
                res = await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating User: {ex.Message}");
            }

            Console.WriteLine("res @res");
        }
    }

    public async Task DeleteUserAsync(string username)
    {
        var query = "DELETE FROM Users WHERE UserName = @UserName";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { UserName = username });
        }
    }
    

    public async Task<User?> GetUserByNameAsync(string name)
    {
        var query = "SELECT * FROM Users WHERE Name = @Name";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Name = name });
        }
    }

    public async Task<Boolean> IsIdPresent(int id)
    {


        using (var connection = _context.CreateConnection())
        {
            var User = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
            if (User is null)
            {
                return false;
            }
            return true;
        }
    }

    public async Task<String> GetUserTypeByIdAsync(int id)
    {
        var query = "SELECT UserType FROM Users WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<string>(query, new { Id = id });
        }
    }

    public async Task RegisterUserAsync(User user)
    {
        var query = "AddUser";

        var parameters = new DynamicParameters();
        parameters.Add("username", user.UserName); 
        parameters.Add("passcode", user.Passcode);

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }

    public async Task<User?> GetUserByCredentialsAsync(string userName, string passcode)
    {
        var query = "GetUserByCredentials";

        var parameters = new DynamicParameters();
        parameters.Add("username", userName);   
        parameters.Add("passcode", passcode);

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<User>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}


