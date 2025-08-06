using ContosoPizza.Data;
using ContosoPizza.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;


namespace ContosoPizza.Repositories;

public class CustomerRepository
{
    private readonly DapperContext _context;

    public CustomerRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        var query = "SELECT * FROM Customers";

        using (var connection = _context.CreateConnection())
        {
            var Customers = await connection.QueryAsync<Customer>(query);
            return Customers;
        }
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        var query = "SELECT * FROM Customers WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
        }
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        var query = "INSERT INTO Customers (Id, Name) VALUES (@Id, @Name);";

        using (var connection = _context.CreateConnection())
        {
            customer.Id = await connection.QuerySingleAsync<int>(query, customer);
        }
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        var query = "UPDATE Customers SET Name = @Name WHERE Id = @id";

        using (var connection = _context.CreateConnection())
        {
            Object res = null;
            try
            {
                res = await connection.ExecuteAsync(query, customer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Customer: {ex.Message}");
            }

            Console.WriteLine("res @res");
        }
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var query = "DELETE FROM Customers WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }

    public async Task<Customer?> GetCustomerByNameAsync(string name)
    {
        var query = "SELECT * FROM Customers WHERE Name = @Name";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Name = name });
        }
    }

    public async Task<Boolean> IsIdPresent(int id)
    {


        using (var connection = _context.CreateConnection())
        {
            var Customer = await connection.QueryFirstOrDefaultAsync<Customer>("SELECT * FROM Customers WHERE Id = @Id", new { Id = id });
            if (Customer is null)
            {
                return false;
            }
            return true;
        }
    }
}


