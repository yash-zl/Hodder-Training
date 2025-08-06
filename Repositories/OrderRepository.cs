using ContosoPizza.Data;
using ContosoPizza.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;


namespace ContosoPizza.Repositories;

public class OrderRepository
{
    private readonly DapperContext _context;

    public OrderRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        var query = "SELECT * FROM Orders";

        using (var connection = _context.CreateConnection())
        {
            var Orders = await connection.QueryAsync<Order>(query);
            return Orders;
        }
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        var query = "SELECT * FROM Orders WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Order>(query, new { Id = id });
        }
    }

    public async Task AddOrderAsync(Order order)
    {

        Console.WriteLine($"Adding order with {order.Id + " " + order.PizzaId + " " + order.CustomerId + " " + order.DateTimePlaced}");
        var query = "INSERT INTO Orders (id, pizza_id, customer_id, datetime_placed) VALUES (@Id, @PizzaId, @CustomerId, @DateTimePlaced);";

        using (var connection = _context.CreateConnection())
        {
            var res = await connection.ExecuteAsync(query, order);

            Console.WriteLine(res);
        }
    }

    public async Task UpdateOrderAsync(Order order)
    {
        var query = "UPDATE Orders SET Name = @Name WHERE Id = @id";

        using (var connection = _context.CreateConnection())
        {
            Object res = null;
            try
            {
                res = await connection.ExecuteAsync(query, order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Order: {ex.Message}");
            }

            Console.WriteLine("res @res");
        }
    }

    public async Task DeleteOrderAsync(int id)
    {
        var query = "DELETE FROM Orders WHERE Id = @Id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }

    public async Task<Order?> GetOrderByNameAsync(string name)
    {
        var query = "SELECT * FROM Orders WHERE Name = @Name";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Order>(query, new { Name = name });
        }
    }

    public async Task<Boolean> IsIdPresent(int id)
    {


        using (var connection = _context.CreateConnection())
        {
            var Order = await connection.QueryFirstOrDefaultAsync<Order>("SELECT * FROM Orders WHERE Id = @Id", new { Id = id });
            if (Order is null)
            {
                return false;
            }
            return true;
        }
    }

    public async Task<IEnumerable<Order>> GetOrdersByDateTimePlaced(bool ascending)
    {
        var query = "SELECT * FROM Orders ORDER BY DateTimePlaced " + (ascending ? "ASC" : "DESC");

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Order>(query);
        }
    }

    public async Task<IEnumerable<Order>> GetOrdersByTotalPrice(bool ascending)
    {
        var query = "SELECT * FROM Orders ORDER BY TotalPrice " + (ascending ? "ASC" : "DESC");

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Order>(query);
        }
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId)
    {
        var query = "SELECT * FROM Orders WHERE CustomerId = @CustomerId";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Order>(query, new { CustomerId = customerId });
        }
    }

    public async Task<IEnumerable<OrderItem>> GetOrdersByCustomerId(int orderId)
    {
        var query = "SELECT * FROM OrderItems WHERE OrderId = @OrderId";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Order>(query, new { OrderId = orderId });
        }
    }
}




