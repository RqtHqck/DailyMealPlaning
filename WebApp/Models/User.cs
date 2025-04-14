using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
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
        
        // Вычисляемое свойство для калорий
        // Формула: BMR = 447.593 + 9.247 * Weight + 3.098 * Height - 4.330 * Age;
        // ARM (коэффициент активности) выбирается в зависимости от уровня активности
        // Рекомендуемое потребление калорий = BMR * ARM
        public decimal Calories => CalculateCalories();

        private decimal CalculateCalories()
        {
            // Harris-Бенедикт для женщин
            decimal bmr = 447.593M + (9.247M * Weight) + (3.098M * Height) - (4.330M * Age);
            decimal multiplier = ActivityLevel switch 
            {
                ActivityLevel.Low => 1.2M,       // Низкая активность
                ActivityLevel.Moderate => 1.375M, // Нормальная активность
                ActivityLevel.Medium => 1.55M,    // Средняя активность
                ActivityLevel.High => 1.725M,     // Высокая активность
                _ => 1M
            };
            return Math.Round(bmr * multiplier, 2);
        }
    }

    public enum ActivityLevel
    {
        Low,
        Moderate,
        Medium,
        High
    }
}