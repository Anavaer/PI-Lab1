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

        public ActionResult Registry(string genderFilter = "Male") {
            var missingPeople = dataBase.MissingPeople.AsQueryable().Where(
                person => person.Gender == (genderFilter == "Male" ? "M" : "F")
            );
        
            return View(missingPeople.ToList());
        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public ActionResult CreateEntry(NewEntryModels data) {
            try {
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
            } catch {
                return View("Create");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id) {
            var missingPerson = dataBase.MissingPeople
                .Where(person => person.Id == id).FirstOrDefault();

            return View(missingPerson);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditEntry(int id, NewEntryModels data) {
            try {
                var personToEdit = dataBase.MissingPeople
                    .Where(p => p.Id == id).FirstOrDefault();

                personToEdit.Name = data.Name;
                personToEdit.Surname = data.Surname;
                personToEdit.Gender = data.Gender;
                personToEdit.Age = data.Age;
                personToEdit.City = data.City;
                personToEdit.PhotoUrl = data.PhotoUrl;

                dataBase.SaveChanges();

                return RedirectToAction("Registry");
            } catch {
                return View("Registry");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id) {
            try {
                var missingPerson = dataBase.MissingPeople
                    .Where(person => person.Id == id).FirstOrDefault();

                dataBase.MissingPeople.Remove(missingPerson);
                dataBase.SaveChanges();
                return RedirectToAction("Registry");
            } catch {
                return View("Registry");
            }
        }
    }


}