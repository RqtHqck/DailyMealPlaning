using System.Xml.Linq;
using WebApp.Models;

namespace WebApp.Services.Data;

public class XmlProductService
{
    private readonly XmlProductsFileLoader _productsFileLoader;

    public XmlProductService(XmlProductsFileLoader productsFileLoader)
    {
        _productsFileLoader = productsFileLoader;
    }

    
    public List<Product> GetProducts(string categoryName)
    {
        var doc = _productsFileLoader.Document;
        var category = doc.Root?.Elements("Category")
            .FirstOrDefault(c => 
                (string)c.Attribute("name")?.Value == categoryName);

        if (category == null) return new List<Product>();

        return category.Elements("Product")
            .Select(p => new Product
            {
                Name = (string)p.Element("Name"),
                Gramms = XmlDecimalParser.ParseDecimalSafe(p.Element("Gramms")),
                Protein = XmlDecimalParser.ParseDecimalSafe(p.Element("Protein")),
                Fats = XmlDecimalParser.ParseDecimalSafe(p.Element("Fats")),
                Carbs = XmlDecimalParser.ParseDecimalSafe(p.Element("Carbs")),
                Calories = XmlDecimalParser.ParseDecimalSafe(p.Element("Calories"))
            })
            .Where(p => p != null)
            .ToList();
    }
    
    
    public void AddProduct(string categoryName, Product product)
    {
        var doc = _productsFileLoader.Document;
    
        // Находим существующую категорию
        var category = doc.Root!.Elements("Category")
            .FirstOrDefault(c => 
                (string)c.Attribute("name") == categoryName);

        if (category == null)
        {
            throw new ArgumentException($"Категория '{categoryName}' не найдена");
        }

        // Добавляем продукт
        category.Add(new XElement("Product",
            new XElement("Name", product.Name),
            new XElement("Gramms", product.Gramms),
            new XElement("Protein", product.Protein),
            new XElement("Fats", product.Fats),
            new XElement("Carbs", product.Carbs),
            new XElement("Calories", product.Calories)
        ));
    
        _productsFileLoader.Save(doc);
    }

    public void DeleteProduct(string categoryName, string productName)
    {
        var doc = _productsFileLoader.Document;
        var category = doc.Root!.Elements("Category")
            .FirstOrDefault(c => (string)c.Attribute("name") == categoryName);
        if (category == null) throw new Exception("Категория не найдена.");

        var product = category.Elements("Product")
            .FirstOrDefault(p => (string)p.Element("Name") == productName);
        if (product == null) throw new Exception("Продукт не найден.");

        product.Remove();
        _productsFileLoader.Save(doc);
    }
}
