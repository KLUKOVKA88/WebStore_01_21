using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers 
{
    public class HomeController : Controller
    {
        //контроллер нужен для того, чтобы обработать входящее подключение
        // на каждый контроллер должно быть свое представление Views

        public IActionResult Index() => View("SecondView");        

        public IActionResult SecondAction()
        {
            return Content("Second controller action");
        }
    }
}
