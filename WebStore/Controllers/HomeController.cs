using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> __Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 37, EmpDate = "2012/05/16" },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Петр", Patronymic = "Петрович", Age = 25, EmpDate = "2019/11/01" },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 27, EmpDate = "2020/01/23" },
        };

        //контроллер нужен для того, чтобы обработать входящее подключение
        // на каждый контроллер должно быть свое представление Views

        public IActionResult Index() => View(/*"SecondView"*/);

        public IActionResult SecondAction()
        {
            return Content("Second controller action");
        }

        public IActionResult Employees()
        {
            return View(__Employees);
        }

        public IActionResult EmployeeCard(int Id)
        {
            List<object> exactEmployee = new List<object>();
            foreach (var employee in __Employees)
            {
                if (employee.Id == Id)
                {
                    ViewBag.Message = employee.EmpDate;
                    exactEmployee.Add(employee.LastName);
                    exactEmployee.Add(employee.FirstName);
                    exactEmployee.Add(employee.Patronymic);
                    break;
                }
            }
            return View(exactEmployee);
        }
    }
}
