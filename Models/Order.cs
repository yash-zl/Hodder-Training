namespace ContosoPizza.Models;

public class Order
{
    public int Id { get; set; }
    public OrderItems[] OrderItems { get; set; }
    public int TotalPrice { get; set; }
    public int CustomerId { get; set; }
    public string OrderType { get; set; }
    public DateTime DateTimePlaced { get; set; }
}