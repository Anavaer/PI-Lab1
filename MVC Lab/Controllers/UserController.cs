using MVC_Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Lab.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext dataBase = ApplicationDbContext.Create();

        public ActionResult Management() {
            return View(dataBase.Users.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id) {
            var parsedUser = dataBase.Users
                .Where(person => person.Id == id).FirstOrDefault();

            return View(parsedUser);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id, ApplicationUser data) {
            try {
                var userToEdit = dataBase.Users
                    .Where(p => p.Id == id).FirstOrDefault();

                userToEdit.UserName = data.UserName;
                userToEdit.Email = data.Email;

                dataBase.SaveChanges();

                return RedirectToAction("Registry");
            }
            catch
            {
                return View("Registry");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id) {
            try
            {
                var parsedUser = dataBase.Users
                    .Where(user => user.Id == id).FirstOrDefault();

                dataBase.Users.Remove(parsedUser);
                dataBase.SaveChanges();
                return RedirectToAction("Management");
            }
            catch
            {
                return View("Management");
            }
        }
    }
}