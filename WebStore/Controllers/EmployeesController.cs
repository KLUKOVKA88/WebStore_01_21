using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private List<Employee> _Employees;

        public EmployeesController()
        {
            _Employees = TestData.Employees;
        }

        public IActionResult Index() => View(_Employees);

        public IActionResult Details(int id) //htpp://localhost:5000/employees/details/2
        {
            var employee = _Employees.FirstOrDefault(e => e.Id == id);
            if (employee is not null)
                return View(employee);
            return NotFound();               
        }


        //public IActionResult EmployeeCard(int Id)
        //{
        //    List<object> exactEmployee = new List<object>();
        //    foreach (var employee in __Employees)
        //    {
        //        if (employee.Id == Id)
        //        {
        //            ViewBag.Message = employee.EmpDate;
        //            exactEmployee.Add(employee.LastName);
        //            exactEmployee.Add(employee.FirstName);
        //            exactEmployee.Add(employee.Patronymic);
        //            break;
        //        }
        //    }
        //    return View(exactEmployee);
        //}
    }
}
