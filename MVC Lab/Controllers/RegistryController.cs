using MVC_Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Lab.Controllers
{
    public class RegistryController : Controller
    {
        private readonly ApplicationDbContext dataBase = ApplicationDbContext.Create();

        // GET: Registry
        public ActionResult Registry(string gender = "M")
        {
            var missingPeople = dataBase.MissingPeople.AsQueryable().Where(
                person => person.Gender == gender
            );
        
            return View(missingPeople.ToList());
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public ActionResult CreateEntry(NewEntryModels data)
        {
            try
            {
                var missingPersonEntry = new NewEntryModels()
                {
                    Name = data.Name,
                    Surname = data.Surname,
                    Gender = data.Gender,
                    Age = data.Age,
                    City = data.City,
                    PhotoUrl = data.PhotoUrl
                };

                dataBase.MissingPeople.Add(missingPersonEntry);
                dataBase.SaveChanges();

                return RedirectToAction("Registry");
            }
            catch
            {
                return View("Create");
            }
        }
    }


}