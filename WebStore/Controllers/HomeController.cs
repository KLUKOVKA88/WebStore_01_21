using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        //контроллер нужен для того, чтобы обработать входящее подключение
        // на каждый контроллер должно быть свое представление Views

        public IActionResult Index() => View();               
           
    }
}
