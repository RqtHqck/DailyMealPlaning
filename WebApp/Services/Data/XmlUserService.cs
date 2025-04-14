using System;
using System.Globalization;
using System.Xml.Linq;
using WebApp.Models;

namespace WebApp.Services.Data
{
    public class XmlUserService
    {
        private readonly XmlUserFileLoader _userFileLoader;

        public XmlUserService(XmlUserFileLoader userFileLoader)
        {
            _userFileLoader = userFileLoader;
        }

        public User GetUser()
        {
            var doc = _userFileLoader.Document;
            if (doc?.Root == null)
                return new User();

            var userElement = doc.Root.Element("User");
            if (userElement != null)
            {
                return new User
                {
                    Weight = decimal.TryParse(userElement.Element("Weight")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal weight) ? weight : 0,
                    Height = decimal.TryParse(userElement.Element("Height")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal height) ? height : 0,
                    Age = int.TryParse(userElement.Element("Age")?.Value, out int age) ? age : 0,
                    ActivityLevel = Enum.TryParse(userElement.Element("ActivityLevel")?.Value, out ActivityLevel level) ? level : ActivityLevel.Low
                    // Calories НЕ загружаем, оно вычисляется автоматически
                };
            }

            return new User();
        }


        public void SaveUser(User user)
        {
            var doc = _userFileLoader.Document ?? new XDocument(new XElement("Db"));
            var root = doc.Root ?? new XElement("Db");

            // Удаляем старые данные пользователя, если они есть
            root.Element("User")?.Remove();

            // Создаём новый узел <User> с данными
            var userElement = new XElement("User",
                new XElement("Weight", user.Weight),
                new XElement("Height", user.Height),
                new XElement("Age", user.Age),
                new XElement("ActivityLevel", user.ActivityLevel.ToString()),
                new XElement("Calories", user.Calories.ToString("0.00", CultureInfo.InvariantCulture)) // ⬅ ДОБАВЛЯЕМ Calories
            );

            root.Add(userElement);
            _userFileLoader.Save(doc);
        }

    }
}
