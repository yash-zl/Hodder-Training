using System.Runtime.InteropServices;

namespace ContosoPizza.Models;

public class Order
{
    public int? Id { get; set; }
    public int? TotalPrice { get; set; }
    public string UserName { get; set; }
    public string OrderType { get; set; }
    public DateTime? TimeDate { get; set; }
    public List<OrderItem> OrderItems { get; set; }

    public void print()
    {
        Console.WriteLine($"Order ID: {Id}, Total Price: {TotalPrice}, UserName: {UserName}, OrderType: {OrderType}, TimeDate: {TimeDate}");
        if (OrderItems != null)
        {
            foreach (var item in OrderItems)
            {
                Console.WriteLine($"OrderItem - PizzaId: {item.PizzaId}, Quantity: {item.Quantity}, PizzaTotalPrice: {item.PizzaTotalPrice}");
            }
        }
    }
}

public class OrderItem
{
    public int? OrderId { get; set; }
    public int PizzaId { get; set; }
    public int Quantity { get; set; }
    public int? PizzaTotalPrice { get; set; }
}