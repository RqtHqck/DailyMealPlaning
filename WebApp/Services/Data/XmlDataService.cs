using System.Globalization;
using System.Xml.Linq;
using WebApp.Models;

namespace WebApp.Services.Data;

public class XmlDataService
{
    private readonly string _xmlPath;

    public XmlDataService(IWebHostEnvironment env)
    {
        _xmlPath = Path.Combine(env.WebRootPath, "App_Data/products.xml");
        InitializeFile();
    }

    private void InitializeFile()
    {
        if (!File.Exists(_xmlPath))
        {
            new XDocument(
                new XElement("Db",
                    new XElement("Category",
                        new XAttribute("name", "Пример категории"),
                        new XElement("Product",
                            new XElement("Name", "Пример продукта"),
                            new XElement("Gramms", 100),
                            new XElement("Protein", 0.00),
                            new XElement("Fats", 0.00),
                            new XElement("Carbs", 0.50),
                            new XElement("Calories", 225)
                        )
                    )
                )
            ).Save(_xmlPath);
        }
    }

    
    // Получение всех категорий
    public List<Category> GetCategories()
    {
        var doc = XDocument.Load(_xmlPath);
        return doc.Root.Elements("Category")
            .Select(c => new Category
            {
                Name = (string)c.Attribute("name"),
                Description = (string)c.Attribute("description"),
                Products = c.Elements("Product")
                    .Select(p =>
                    {
                        try
                        {
                            return new Product
                            {
                                Name = (string)p.Element("Name"),
                                Gramms = ParseDecimalSafe(p.Element("Gramms")),
                                Protein = ParseDecimalSafe(p.Element("Protein")),
                                Fats = ParseDecimalSafe(p.Element("Fats")),
                                Carbs = ParseDecimalSafe(p.Element("Carbs")),
                                Calories = ParseDecimalSafe(p.Element("Calories"))
                            };
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при парсинге продукта: {ex.Message}");
                            return null; // Пропустить повреждённый продукт
                        }
                    })
                    .Where(p => p != null)
                    .ToList()
            }).ToList();
    }
    
    
    // Парсинг значений Xml
    private decimal ParseDecimalSafe(XElement element)
    {
        if (element == null || string.IsNullOrWhiteSpace(element.Value))
            return 0;

        var raw = element.Value.Trim().Replace(",", ".");

        if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;

        throw new FormatException($"Невозможно распарсить '{element.Value}' как decimal.");
    }


    // Добавление продукта
    public void AddProduct(string categoryName, Product product)
    {
        var doc = XDocument.Load(_xmlPath);
        var category = doc.Root.Elements("Category")
            .FirstOrDefault(c => (string)c.Attribute("name") == categoryName);

        if (category == null)
        {
            category = new XElement("Category",
                new XAttribute("name", categoryName)
            );
            doc.Root.Add(category);
        }

        category.Add(new XElement("Product",
            new XElement("Name", product.Name),
            new XElement("Gramms", product.Gramms),
            new XElement("Protein", product.Protein.ToString("0.00")),
            new XElement("Fats", product.Fats.ToString("0.00")),
            new XElement("Carbs", product.Carbs.ToString("0.00")),
            new XElement("Calories", product.Calories)
        ));

        doc.Save(_xmlPath);
    }
    
    
    // Удаление продукта из категории
    public void DeleteProduct(string categoryName, string productName)
    {
        var doc = XDocument.Load(_xmlPath);
        var category = doc.Root.Elements("Category")
            .FirstOrDefault(c => (string)c.Attribute("name") == categoryName);

        if (category == null)
            throw new Exception("Категория не найдена.");

        var product = category.Elements("Product")
            .FirstOrDefault(p => (string)p.Element("Name") == productName);

        if (product == null)
            throw new Exception("Продукт не найден.");

        product.Remove();
        doc.Save(_xmlPath);
    }
    
    
    // Метод добавления категории
    public void AddCategory(string categoryName, string description="")
    {
        var doc = XDocument.Load(_xmlPath);
        var category = new XElement("Category",
            new XAttribute("name", categoryName),
            new XAttribute("description", description )

        );
        doc.Root.Add(category);
        doc.Save(_xmlPath);
    }

    // Метод удаления категории
    public void DeleteCategory(string categoryName)
    {
        var doc = XDocument.Load(_xmlPath); // Загрузить XML документ
        var categoryElement = doc.Root.Elements("Category")
            .FirstOrDefault(c => c.Attribute("name")?.Value == categoryName); // Поиск категории по имени

        if (categoryElement != null)
        {
            categoryElement.Remove(); // Удаление категории
            doc.Save(_xmlPath); // Сохранение изменений
        }
        else
        {
            throw new Exception("Категория не найдена.");
        }
    }



}