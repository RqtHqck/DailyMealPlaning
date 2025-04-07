using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services.Data;

namespace WebApp.Controllers;

public class MealController: Controller
{
    private readonly XmlMealPlanService _mealPlanService;

    public MealController(XmlMealPlanService mealPlanService)
    {
        _mealPlanService = mealPlanService;
    }
    
    public IActionResult Index()
    {
        var mealPlan = _mealPlanService.GetMealPlan();
        
        return View(mealPlan.MealItems);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.MealTypes = Enum.GetValues(typeof(MealType)).Cast<MealType>();
        return View(new MealItem());
    }

    [HttpPost]
    public IActionResult Create(MealItem item)
    {
        var plan = _mealPlanService.GetMealPlan();
        plan.MealItems.Add(item);
        _mealPlanService.SaveMealPlan(plan);
        return RedirectToAction("Index");
    }
    

}