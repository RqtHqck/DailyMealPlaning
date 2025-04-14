using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services.Data;

namespace WebApp.Controllers;

public class MealController(XmlProductService productService, XmlMealPlanService _mealPlanService): Controller
{
    
    public IActionResult Index()
    {
        var mealPlan = _mealPlanService.GetMealPlan();
        
        return View(mealPlan.MealItems);
    }

    [HttpGet]
    public IActionResult Create()
    {
        
        ViewBag.MealTypes = Enum.GetValues(typeof(MealType)).Cast<MealType>();
        ViewBag.AllProducts = productService.GetAllProducts(); // Получаем все продукты
        return View(new MealItem());
    }

    [HttpPost]
    public IActionResult Create(MealItem item)
    {
        
        foreach (var p in item.Products)
        {
            Console.WriteLine($"Product: {p.Name}, {p.Protein}, {p.Fats}, {p.Carbs}, {p.Calories}");
        }

        var plan = _mealPlanService.GetMealPlan();
        plan.MealItems.Add(item);
        _mealPlanService.SaveMealPlan(plan);
        return RedirectToAction("Index");
    }
    

}