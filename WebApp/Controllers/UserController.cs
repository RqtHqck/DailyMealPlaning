using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services.Data;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly XmlUserService _userService;

        public UserController(XmlUserService userService)
        {
            _userService = userService;
        }

        // Отображение формы с текущими данными пользователя
        [HttpGet]
        public IActionResult Index()
        {
            var userData = _userService.GetUser();
            return View(userData);
        }

        // Обработка отправки формы с данными пользователя
        [HttpPost]
        public IActionResult Index(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            _userService.SaveUser(user);
            return RedirectToAction("Index");
        }
    }
}