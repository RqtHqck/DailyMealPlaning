using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services.Data;

namespace WebApp.Controllers
{
    public class MealController : Controller
    {
        private readonly XmlProductService _productService;
        private readonly XmlMealPlanService _mealPlanService;

        public MealController(XmlProductService productService, XmlMealPlanService mealPlanService)
        {
            _productService = productService;
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
            ViewBag.AllProducts = _productService.GetAllProducts();
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

        [HttpPost]
        public IActionResult DeleteMeal(int index)
        {
            var plan = _mealPlanService.GetMealPlan();
            if (index >= 0 && index < plan.MealItems.Count)
            {
                plan.MealItems.RemoveAt(index);
                _mealPlanService.SaveMealPlan(plan);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteProduct(int mealIndex, int productIndex)
        {
            var plan = _mealPlanService.GetMealPlan();
            if (mealIndex >= 0 && mealIndex < plan.MealItems.Count)
            {
                var meal = plan.MealItems[mealIndex];
                if (productIndex >= 0 && productIndex < meal.Products.Count)
                {
                    meal.Products.RemoveAt(productIndex);
                    _mealPlanService.SaveMealPlan(plan);
                }
            }
            return RedirectToAction("Index");
        }
        
        // New Action for saving the meal plan to a file
        [HttpGet]
        public IActionResult DownloadMealPlan()
        {
            var mealPlan = _mealPlanService.GetMealPlan();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "user.xml"); // Define the file path where you want to save the XML file
            _mealPlanService.SaveMealPlanToFile(mealPlan, filePath);

            // Return the file for download
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/xml", "user.xml");
        }
    }
}
