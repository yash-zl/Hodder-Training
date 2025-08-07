using System.Runtime.InteropServices;

namespace ContosoPizza.Models;

public class AdminHomeData
{
    public List<Order> Orders { get; set; } = new List<Order>();
    public List<Pizza> Pizzas { get; set; } = new List<Pizza>();
    public List<User> Users { get; set; } = new List<User>();
}