using System.ComponentModel;
using System.Xml.Serialization;

namespace WebApp.Models;

public class Product
{
    [XmlElement("Name")]
    [DisplayName("Название")]
    public string Name { get; set; }

    [XmlElement("Gramms")]
    [DisplayName("Граммовка")]
    public decimal Gramms { get; set; } = 100; // По умолчанию 100 г

    [XmlElement("Protein")]
    [DisplayName("Белки")]
    public decimal Protein { get; set; }

    [XmlElement("Fats")]
    [DisplayName("Жиры")]
    public decimal Fats { get; set; }

    [XmlElement("Carbs")]
    [DisplayName("Углеводы")]
    public decimal Carbs { get; set; }

    [XmlElement("Calories")]
    [DisplayName("Калории")]
    public decimal Calories { get; set; }
}