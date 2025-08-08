using ContosoPizza.Models;
using ContosoPizza.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoPizza.Services;

public class OrderService
{
    static int nextId = 0;

    private readonly OrderRepository _repository;


    public OrderService(OrderRepository repository)
    {
        _repository = repository;
    }
    public List<Order> GetAll() => _repository.GetAllOrdersAsync().Result.ToList();

    public Order? GetByOrderId(int id) => _repository.GetOrderByIdAsync(id).Result;

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string username)
    {
        var orders = _repository.GetOrdersByUserNameAsync(username).Result;
        return orders != null ? new List<Order> { orders } : new List<Order>();
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync(int pageNumber, int pageSize = 10, bool asc = true, bool byprice = true)
    {
        var orders = _repository.GetOrders(pageNumber, asc, byprice, pageSize).Result;
        return orders.ToList();
    }

    public async Task<int> GetTotalPages(int pageSize = 10, bool asc = true, bool byprice = true)
    {
        Console.WriteLine($"Calculating total pages with pageSize: {pageSize}, asc: {asc}, byprice: {byprice}");
        return await _repository.GetTotalPagesAsync(asc, byprice, pageSize);
    }

    public async Task Add(Order order)
    {
        order.Id = nextId++;
        Console.WriteLine($"Adding order with ID: {order.Id}");
        await _repository.AddOrderAsync(order);
    }

    public void Delete(int id)
    {
        _repository.DeleteOrderAsync(id).Wait();
    }

    public void Update(Order order)
    {
        _repository.UpdateOrderAsync(order).Wait();
    }
}