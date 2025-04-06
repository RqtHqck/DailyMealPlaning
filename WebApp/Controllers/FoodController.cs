using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services.Data;

namespace WebApp.Controllers;

public class FoodController : Controller
{
    private readonly XmlDataService _dataService;

    public FoodController(XmlDataService dataService)
    {
        _dataService = dataService;
    }

    // Отображение всех категорий
    public IActionResult Index()
    {
        var categories = _dataService.GetCategories();
        return View(categories);
    }

    // Действие для добавления категории
    [HttpPost]
    public IActionResult AddCategory(string categoryName)
    {
        if (!string.IsNullOrEmpty(categoryName))
        {
            _dataService.AddCategory(categoryName); // Логика добавления категории
        }

        return RedirectToAction("Index"); // Перенаправление на страницу с категориями
    }

    // Действие для удаления категории
[HttpPost]
public IActionResult DeleteCategory(string categoryName)
{
    try
    {
        _dataService.DeleteCategory(categoryName);
        return RedirectToAction("Index"); // Перенаправление на ту же страницу
    }
    catch (Exception ex)
    {
        TempData["ErrorMessage"] = "Не удалось удалить категорию: " + ex.Message;
        return RedirectToAction("Index");
    }
}

    // [HttpPost]
    // public IActionResult AddProduct(string categoryName, Product product)
    // {
    //     _dataService.AddProduct(categoryName, product);
    //     return RedirectToAction("Index");
    // }
}