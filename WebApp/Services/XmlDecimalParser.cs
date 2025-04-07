using System.Globalization;
using System.Xml.Linq;

namespace WebApp.Services.Data;

public static class XmlDecimalParser
{
    public static decimal ParseDecimalSafe(XElement? element)
    {
        if (element == null || string.IsNullOrWhiteSpace(element.Value)) return 0;
        var raw = element.Value.Trim().Replace(",", ".");
        if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;

        throw new FormatException($"Невозможно распарсить '{element.Value}' как decimal.");
    }
}
