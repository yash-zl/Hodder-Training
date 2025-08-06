namespace ContosoPizza.Models;

public class OrderItems
{
    public int OrderId { get; set; }
    public int PizzaId { get; set; }
    public int Quantity { get; set; }
}