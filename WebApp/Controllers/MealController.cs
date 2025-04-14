using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        [HttpPost]
        public IActionResult UpdateMeal(int mealIndex, MealItem updatedMeal)
        {
            var plan = _mealPlanService.GetMealPlan();
            if (mealIndex < 0 || mealIndex >= plan.MealItems.Count)
            {
                return RedirectToAction("Index");
            }

            // Получаем исходный прием для обновления
            var origMeal = plan.MealItems[mealIndex];

            // Обновляем название и тип приема
            origMeal.Name = updatedMeal.Name;
            origMeal.Type = updatedMeal.Type;

            // Для каждого продукта, переданного в форме обновления,
            // обновляем только значение веса в исходном приеме, сопоставляя по имени продукта.
            foreach (var updatedProduct in updatedMeal.Products)
            {
                var origProduct = origMeal.Products.FirstOrDefault(p => p.Name == updatedProduct.Name);
                if (origProduct != null)
                {
                    origProduct.Gramms = updatedProduct.Gramms;
                    // Если требуется, здесь можно добавить пересчет остальных показателей,
                    // например: origProduct.Protein = CalculateProtein(origProduct, updatedProduct.Gramms);
                }
                else
                {
                    // Если продукт новый, можно добавить его в коллекцию
                    origMeal.Products.Add(updatedProduct);
                }
            }

            _mealPlanService.SaveMealPlan(plan);
            return RedirectToAction("Index");
        }
    }
}
