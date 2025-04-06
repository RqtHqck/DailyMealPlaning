using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class MealPlan
{
    public DateTime Date { get; set; } = DateTime.Today;
    public List<Meal> Meals { get; set; } = new();
}


public class Meal
{
    public MealType Type { get; set; }
    public List<MealItem> Items { get; set; } = new();
}


public class MealItem
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    [Range(1, 1000, ErrorMessage = "Вес должен быть от 1 до 1000 г")]
    public decimal Weight { get; set; }
}


public enum MealType
{
    Breakfast,
    Lunch,
    Dinner,
    Custom
}
