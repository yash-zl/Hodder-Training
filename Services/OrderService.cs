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

    public Order? Get(int id) => _repository.GetOrderByIdAsync(id).Result;

    public async Task Add(Order order)
    {
        order.Id = nextId++;
        order.DateTimePlaced = DateTime.UtcNow; // Set the current time as the order placed time
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