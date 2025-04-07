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

    
    // Действие для добавления продукта
    [HttpPost]
    public IActionResult AddProduct(string categoryName, Product product)
    {
        try
        {
            _dataService.AddProduct(categoryName, product);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Ошибка при добавлении продукта: " + ex.Message;
        }

        return RedirectToAction("Index");
    }
    
    
    [HttpPost]
    public IActionResult DeleteProduct(string categoryName, string productName)
    {
        try
        {
            _dataService.DeleteProduct(categoryName, productName);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Ошибка при удалении продукта: " + ex.Message;
        }

        return RedirectToAction("Index");
    }


}