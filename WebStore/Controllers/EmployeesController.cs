﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    //[Route("staff")]
    public class EmployeesController : Controller
    {
        private IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData EmployeesData) => _EmployeesData = EmployeesData; 

        //[Route("all")]
        public IActionResult Index() => View(_EmployeesData.Get());

        //[Route("info(id:{id})")]
        public IActionResult Details(int id) //htpp://localhost:5000/employees/details/2
        {
            var employee = _EmployeesData.Get(id);
            if (employee is not null)
                return View(employee);
            return NotFound();               
        }

        public IActionResult Create() => View("Edit", new EmployeeViewModel());

        #region Edit
        public IActionResult Edit(int id)
        {
            if (id <= 0) return BadRequest();

            var employee = _EmployeesData.Get(id);

            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                MiddleName = employee.Patronymic,
                Age = employee.Age
            });
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.Name,
                Patronymic = model.MiddleName,
                Age = model.Age
            };

            if (employee.Id == 0)
                _EmployeesData.Add(employee);
            else
                _EmployeesData.Update(employee);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var employee = _EmployeesData.Get(id);

            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                MiddleName = employee.Patronymic,
                Age = employee.Age
            });
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            return RedirectToAction("Index");
        }
        #endregion

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
