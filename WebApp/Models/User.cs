using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class User
{
    [Required(ErrorMessage = "Введите вес")]
    [Range(30, 300, ErrorMessage = "Вес должен быть от 30 до 300 кг")]
    public decimal Weight { get; set; }

    [Required(ErrorMessage = "Введите рост")]
    [Range(100, 250, ErrorMessage = "Рост должен быть от 100 до 250 см")]
    public decimal Height { get; set; }

    [Required(ErrorMessage = "Введите возраст")]
    [Range(1, 120, ErrorMessage = "Возраст должен быть от 1 до 120 лет")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Выберите уровень активности")]
    public ActivityLevel ActivityLevel { get; set; }
}

public enum ActivityLevel
{
    Low,
    Moderate,
    High,
    VeryHigh
}
