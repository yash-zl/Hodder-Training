namespace ContosoPizza.Models;

public class Order
{
    public int Id { get; set; }

    public int PizzaId { get; set; }

    public int CustomerId { get; set; }

    public DateTime DateTimePlaced { get; set; }
}