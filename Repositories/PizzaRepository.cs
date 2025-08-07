using System.Data;
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
        try
        {
            var query = "addpizza";
            Console.WriteLine($"Adding pizza with ID: {pizza.Id}, Name: {pizza.Name}, Price: {pizza.Price}, IsVeg: {pizza.IsVeg}, Kcal: {pizza.Kcal}");
            // Console.WriteLine($"Adding pizza with ID: {pizza.Id, pizza.Name, pizza.Price, pizza.IsVeg, pizza.Kcal}");
            var parameters = new DynamicParameters();
            parameters.Add("name", pizza.Name, dbType: DbType.String);
            parameters.Add("price", pizza.Price, dbType: DbType.Int32);
            parameters.Add("kcal", pizza.Kcal, dbType: DbType.Int32);
            parameters.Add("isveg", pizza.IsVeg, dbType: DbType.Boolean);
            Console.WriteLine($"Adding pizza with params: {parameters.Get<string>("name")}, {parameters.Get<bool>("isveg")}, {parameters.Get<int>("price")}, {parameters.Get<int>("kcal")}");

            using (var connection = _context.CreateConnection())
            {
                connection.Execute(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        catch(
            Exception ex
        )
        {
            Console.WriteLine($"Error adding pizza: {ex.Message}");
        }
    }

    public async Task UpdatePizzaAsync(Pizza pizza)
    {
        var query = "UpdatePizza";
        var parameters = new DynamicParameters();
        parameters.Add("id", pizza.Id);
        parameters.Add("name", pizza.Name);
        parameters.Add("isveg", pizza.IsVeg);
        parameters.Add("price", pizza.Price);
        parameters.Add("kcal", pizza.Kcal);

        using (var connection = _context.CreateConnection())
        {
            try
            {
                await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating pizza: {ex.Message}");
                // Re-throw the exception to be handled by the caller
            }
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
}


