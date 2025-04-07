using System.Xml.Linq;
using WebApp.Models;

namespace WebApp.Services.Data;

public class XmlCategoryService
{
    private readonly XmlProductsFileLoader _productsFileLoader;

    public XmlCategoryService(XmlProductsFileLoader productsFileLoader)
    {
        _productsFileLoader = productsFileLoader;
    }

    public List<Category> GetCategories()
    {
        var doc = _productsFileLoader.Document;
        return doc.Root!.Elements("Category")
            .Select(c => new Category
            {
                Name = (string)c.Attribute("name"),
                Description = (string)c.Attribute("description"),
                Products = new() // Без продуктов — это делает ProductService
            }).ToList();
    }

    public void AddCategory(string name, string description = "")
    {
        var doc = _productsFileLoader.Document;
        doc.Root!.Add(new XElement("Category",
            new XAttribute("name", name),
            new XAttribute("description", description)));
        _productsFileLoader.Save(doc);
    }

    public void DeleteCategory(string name)
    {
        var doc = _productsFileLoader.Document;
        var category = doc.Root!.Elements("Category")
            .FirstOrDefault(c => (string)c.Attribute("name") == name);

        if (category == null) throw new Exception("Категория не найдена.");
        category.Remove();
        _productsFileLoader.Save(doc);
    }
}
