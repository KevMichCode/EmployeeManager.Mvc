﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManager.Mvc.Controllers
{
    [Authorize(Roles = "Manager")]
    public class EmployeeManagerController : Controller
    {
        private AppDbContext db = null;

        public EmployeeManagerController(AppDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Returning list of countries
        /// </summary>
        private void FillCountries()
        {
            List<SelectListItem> countries =
                (
                    from c in db.Countries
                    orderby c.Name ascending
                    select new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Name
                    }
                ).ToList();
            ViewBag.Countries = countries;
        }

        /// <summary>
        /// returning list of employees
        /// </summary>
        /// <returns></returns>
        public IActionResult List()
        {
            List<Employee> model =
                (
                    from e in db.Employees
                    orderby e.EmployeeID
                    select e
                ).ToList();
            return View(model);
        }

        /// <summary>
        /// Inserting new employee
        /// </summary>
        /// <returns></returns>
        
        public IActionResult Insert()
        {
            FillCountries();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Insert(Employee model)
        {
            FillCountries();
            if(ModelState.IsValid)
            {
                db.Employees.Add(model);
                db.SaveChanges();
                ViewBag.Message = "Employee inserted successfully";
            }
            return View(model);
        }

        /// <summary>
        /// updating employee
        /// </summary>
        /// <returns></returns>
        
        public IActionResult Update(int id)
        {
            FillCountries();
            Employee model = db.Employees.Find(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Employee model)
        {
            FillCountries();
            if(ModelState.IsValid)
            {
                db.Employees.Update(model);
                db.SaveChanges();
                ViewBag.Message = "Employee update successfully";
            }
            return View(model);
        }

        /// <summary>
        /// Deleting an employee
        /// </summary>
        /// <returns></returns>
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            Employee model = db.Employees.Find(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int employeeID)
        {
            Employee model = db.Employees.Find(employeeID);
            db.Employees.Remove(model);
            db.SaveChanges();
            TempData["Message"] = "Employee deleted successfully";
            return RedirectToAction("List");
        }

        [AllowAnonymous]
        public IActionResult Help()
        {
            return View();
        }
    }
}
