using System.Globalization;
using System.Xml.Linq;
using WebApp.Models;

namespace WebApp.Services.Data;

public class XmlMealPlanService
{
    private readonly XmlUserFileLoader _userFileLoader;

    public XmlMealPlanService(XmlUserFileLoader userFileLoader)
    {
        _userFileLoader = userFileLoader;
    }
    
    public MealsPlan GetMealPlan()
    {
        var doc = _userFileLoader.Document;
        if (doc?.Root == null) return new MealsPlan();

        var mealItems = doc.Root.Element("MealsPlan")?.Elements("MealItem")
            .Select(e => new MealItem
            {
                Name = (string?)e.Attribute("name") ?? string.Empty,
                Type = Enum.TryParse((string?)e.Attribute("type"), out MealType t) ? t : MealType.Custom,
                CustomTypeName = (string?)e.Attribute("customTypeName"),
                Products = e.Elements("Product").Select(p => new Product
                {
                    Name = (string)p.Element("Name"),
                    Gramms = XmlDecimalParser.ParseDecimalSafe(p.Element("Gramms")),
                    Protein = XmlDecimalParser.ParseDecimalSafe(p.Element("Protein")),
                    Fats = XmlDecimalParser.ParseDecimalSafe(p.Element("Fats")),
                    Carbs = XmlDecimalParser.ParseDecimalSafe(p.Element("Carbs")),
                    Calories = XmlDecimalParser.ParseDecimalSafe(p.Element("Calories"))
                }).ToList()
            }).ToList() ?? new List<MealItem>();

        return new MealsPlan { MealItems = mealItems };
    }

    
    public void SaveMealPlan(MealsPlan plan)
    {
        var doc = _userFileLoader.Document ?? new XDocument(new XElement("Db"));
        var root = doc.Root ?? new XElement("Db");

        // Удаляем старый узел <MealsPlan>, но НЕ трогаем <User>!
        root.Element("MealsPlan")?.Remove();

        var mealPlanElement = new XElement("MealsPlan",
            plan.MealItems.Select(m =>
                new XElement("MealItem",
                    new XAttribute("name", m.Name),
                    new XAttribute("type", m.Type.ToString()),
                    string.IsNullOrEmpty(m.CustomTypeName) ? null : new XAttribute("customTypeName", m.CustomTypeName),
                    m.Products.Select(p =>
                        new XElement("Product",
                            new XElement("Name", p.Name),
                            new XElement("Gramms", p.Gramms),
                            new XElement("Protein", p.Protein.ToString("0.00", CultureInfo.InvariantCulture)),
                            new XElement("Fats", p.Fats.ToString("0.00", CultureInfo.InvariantCulture)),
                            new XElement("Carbs", p.Carbs.ToString("0.00", CultureInfo.InvariantCulture)),
                            new XElement("Calories", p.Calories.ToString("0.00", CultureInfo.InvariantCulture))
                        )
                    )
                )
            )
        );

        // Добавляем новый <MealsPlan> без удаления других данных
        root.Add(mealPlanElement);

        _userFileLoader.Save(doc);
    }

}