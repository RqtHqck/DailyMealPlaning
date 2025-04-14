using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services.Data;

namespace WebApp.Controllers;

public class FoodController(XmlProductService productService, XmlCategoryService categoryService)
    : Controller
{
    // Отображение всех категорий
    public IActionResult Index()
    {
        var categories = categoryService.GetCategories();
    
        // Добавляем продукты в каждую категорию
        foreach (var category in categories)
        {
            category.Products = productService.GetProducts(category.Name);
        }
    
        return View(categories);
    }
    
    
    public IActionResult AllProducts()
    {
        var products = productService.GetAllProducts();
        return View(products);
    }
    
    
    // Действие для добавления категории
    [HttpPost]
    public IActionResult AddCategory(string categoryName)
    {
        if (!string.IsNullOrEmpty(categoryName))
        {
            categoryService.AddCategory(categoryName); // Логика добавления категории
        }

        return RedirectToAction("Index"); // Перенаправление на страницу с категориями
    }

    // Действие для удаления категории
    [HttpPost]
    public IActionResult DeleteCategory(string categoryName)
    {
        try
        {
            categoryService.DeleteCategory(categoryName);
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
            productService.AddProduct(categoryName, product);
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
            productService.DeleteProduct(categoryName, productName);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Ошибка при удалении продукта: " + ex.Message;
        }

        return RedirectToAction("Index");
    }


}