using ContosoPizza.Data;
using ContosoPizza.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;


namespace ContosoPizza.Repositories;

public class PizzaRepository
{
    private readonly DapperContext _context;

    public PizzaRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pizza>> GetAllPizzaAsync()
    {
        var query = "SELECT * FROM Pizza";

        using (var connection = _context.CreateConnection())
        {
            var Pizza = await connection.QueryAsync<Pizza>(query);
            return Pizza;
        }
    }

    public async Task<Pizza?> GetPizzaByIdAsync(int id)
    {
        var query = "SELECT * FROM Pizza WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Pizza>(query, new { Id = id });
        }
    }

    public async Task AddPizzaAsync(Pizza pizza)
    {
        var query = "INSERT INTO Pizza (Id, Name, IsGlutenFree) VALUES (@Id, @Name, @IsGlutenFree);";

        using (var connection = _context.CreateConnection())
        {
            pizza.Id = await connection.QuerySingleAsync<int>(query, pizza);
        }
    }

    public async Task UpdatePizzaAsync(Pizza pizza)
    {
        var query = "UPDATE Pizza SET Name = @Name, IsGlutenFree = @IsGlutenFree WHERE Id = @id";

        using (var connection = _context.CreateConnection())
        {
            Object res = null;
            try
            {
                res = await connection.ExecuteAsync(query, pizza);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating pizza: {ex.Message}");
                // Re-throw the exception to be handled by the caller
            }

            Console.WriteLine("res @res");
        }
    }

    public async Task DeletePizzaAsync(int id)
    {
        var query = "DELETE FROM Pizza WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }

    public async Task<Pizza?> GetPizzaByNameAsync(string name)
    {
        var query = "SELECT * FROM Pizza WHERE Name = @Name";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Pizza>(query, new { Name = name });
        }
    }

    public async Task<IEnumerable<Pizza>> GetPizzaByGlutenFreeAsync(bool isGlutenFree)
    {
        var query = "SELECT * FROM Pizza WHERE IsGlutenFree = @IsGlutenFree";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Pizza>(query, new { IsGlutenFree = isGlutenFree });
        }
    }

    public async Task<Boolean> IsIdPresent(int id)
    {


        using (var connection = _context.CreateConnection())
        {
            var pizza = await connection.QueryFirstOrDefaultAsync<Pizza>("SELECT * FROM Pizza WHERE Id = @Id", new { Id = id });
            if (pizza is null)
            {
                return false;
            }
            return true;
        }
    }

    public async Task<IEnumerable<Pizza>> GetPizzasByPrice(bool ascending)
    {
        var query = ascending ? "SELECT * FROM Pizza ORDER BY Price ASC" : "SELECT * FROM Pizza ORDER BY Price DESC";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Pizza>(query);
        }
    }

    public async Task<IEnumerable<Pizza>> GetVegetarianPizzas()
    {
        var query = "SELECT * FROM Pizza WHERE IsVegetarian = true";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Pizza>(query);
        }
    }
}


