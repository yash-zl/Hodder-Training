namespace ContosoPizza.Models;

public class Pizza
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Kcal { get; set; }
    public bool IsVeg { get; set; }
}