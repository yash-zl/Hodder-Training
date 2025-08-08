namespace ContosoPizza.Models;

public class GetOrdersResponse
{
    public List<Order> Orders { get; set; } = new List<Order>();
    public int TotalPages { get; set; }
}