using ContosoPizza.Data;
using ContosoPizza.Models;
using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;


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
            var order = await connection.QueryFirstOrDefaultAsync<Order>(query, new { Id = id });
            return order;
        }
    }

    public async Task<Order?> GetOrdersByUserNameAsync(string userName)
    {
        var query = "SELECT * FROM Orders WHERE UserName = @UserName";

        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Order>(query, new { UserName = userName });
        }
    }

    public async Task AddOrderAsync(Order order)
    {

        order.print();

        try
        {
            var query = "AddOrder";
            var parameters = new DynamicParameters();
            parameters.Add("username", order.UserName);
            parameters.Add("timedate", null);
            parameters.Add("ordertype", order.OrderType);

            var orderitems = new DataTable();
            orderitems.Columns.Add("pizzaid", typeof(int));
            orderitems.Columns.Add("quantity", typeof(int));
            orderitems.Columns.Add("pizzatotalprice", typeof(int));
            foreach (var OrderItem in order.OrderItems)
            {
                var pizzaquantityquery = "GetPriceForPizzaQuantity";
                var pizzaquantityparams = new DynamicParameters();
                pizzaquantityparams.Add("pizzaid", OrderItem.PizzaId, dbType: DbType.Int32);
                pizzaquantityparams.Add("quantity", OrderItem.Quantity, dbType: DbType.Int32);
                pizzaquantityparams.Add("pizzaquantitytotalprice", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);



                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(pizzaquantityquery, pizzaquantityparams, commandType: System.Data.CommandType.StoredProcedure);

                }

                OrderItem.PizzaTotalPrice = pizzaquantityparams.Get<int>("pizzaquantitytotalprice");
                // Consol/e.WriteLine($"PizzaId: {OrderItem.PizzaId}, Quantity: {OrderItem.Quantity}, TotalPrice: {OrderItem.PizzaTotalPrice}");
                orderitems.Rows.Add(OrderItem.PizzaId, OrderItem.Quantity, OrderItem.PizzaTotalPrice);
            }

            foreach (DataRow item in orderitems.Rows)
            {
                Console.WriteLine($"PizzaId: {item["pizzaid"]}, Quantity: {item["quantity"]}, TotalPrice: {item["pizzatotalprice"]}");
            }

            parameters.Add("OrderItems", orderitems.AsTableValuedParameter("orderitemstype"));
            printparams(parameters);
            using (var connection = _context.CreateConnection())
            {
                var res = await connection.ExecuteAsync(query, parameters, commandType: System.Data.CommandType.StoredProcedure);

                Console.WriteLine(res);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding order: {ex.Message}");
            // Handle the exception as needed
        }
    }

    public void printparams(DynamicParameters parameters)
    {
        foreach (var param in parameters.ParameterNames)
        {
            Console.WriteLine($"Parameter: {param}, Value: {parameters.Get<object>(param)}");
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

    public async Task<IEnumerable<Order>> GetOrders(int pageNumber, bool asc, bool byprice, int pageSize)
    {
        var query = "getorders";
        var parameters = new DynamicParameters();
        parameters.Add("pagenumber", pageNumber);
        parameters.Add("pagesize", pageSize);
        parameters.Add("asc", asc);
        parameters.Add("byprice", byprice);

        using (var connection = _context.CreateConnection())
        {
            var Orders = await connection.QueryAsync<Order>(query, parameters, commandType: System.Data.CommandType.StoredProcedure);
            Console.WriteLine($"--Fetched orders for page {pageNumber} with page size {pageSize}, asc: {asc}, byprice: {byprice}");

            Orders.ToList().ForEach(e => { Console.WriteLine(e.Id); });
            return Orders;


        }
    }

    public async Task<int> GetTotalPagesAsync(bool asc, bool byprice, int pageSize)
    {
        try
        {
            Console.WriteLine($"--Calculating total pages with asc: {asc}, byprice: {byprice}, pageSize: {pageSize}");
            var query = "select count(*) from Orders";

            using (var connection = _context.CreateConnection())
            {
                var totalCount = await connection.ExecuteScalarAsync<int>(query);
                return (int)Math.Ceiling((double)totalCount / pageSize);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calculating total pages: {ex.Message}");
            return 0; // or handle the error as needed
        }
    }


}


