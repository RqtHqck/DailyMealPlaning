using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace WebApp.Models;

[XmlRoot("MealsPlan")]
public class MealsPlan
{
    [XmlElement("MealItem")]
    public List<MealItem> MealItems { get; set; } = new();
}

public class MealItem
{
    [XmlAttribute("type")]
    public MealType Type { get; set; }

    [XmlAttribute("customTypeName")]
    public string? CustomTypeName { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement("Product")]
    public List<Product> Products { get; set; } = new();

    public string GetDisplayType() =>
        Type == MealType.Custom ? CustomTypeName ?? "Custom" : Type.ToString();
}


public enum MealType
{
    [XmlEnum("Завтрак")]
    Breakfast,

    [XmlEnum("Обед")]
    Lunch,

    [XmlEnum("Ужин")]
    Dinner,

    [XmlEnum("Настроить")]
    Custom
}