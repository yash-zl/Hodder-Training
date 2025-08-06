using ContosoPizza.Models;
using ContosoPizza.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoPizza.Services;

public class CustomerService
{
    static int nextId = 0;

    private readonly CustomerRepository _repository;


    public CustomerService(CustomerRepository repository)
    {
        _repository = repository;
    }
    public List<Customer> GetAll() => _repository.GetAllCustomersAsync().Result.ToList();

    public Customer? Get(int id) => _repository.GetCustomerByIdAsync(id).Result;

    public async Task Add(Customer customer)
    {
        customer.Id = nextId++;
        Console.WriteLine($"Adding customer with ID: {customer.Id}");  
        await _repository.AddCustomerAsync(customer);
    }

    public void Delete(int id)
    {
        _repository.DeleteCustomerAsync(id).Wait();
    }

    public void Update(Customer customer)
    {
        _repository.UpdateCustomerAsync(customer).Wait();
    }
}