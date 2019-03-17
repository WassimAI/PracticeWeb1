using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PracticeWeb1.Entities;
using PracticeWeb1.Entities.ViewModels;
using PracticeWeb1.Models;

namespace PracticeWeb1.Controllers
{
    public class UserAccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserAccount
        public ActionResult Index()
        {
            var userVMList = new List<UserAccountVM>();

            userVMList = db.userAccounts.ToArray().Select(x => new UserAccountVM(x)).ToList();

            return View(userVMList);
        }

        public ActionResult Register()
        {
            var model = new UserAccountVM();

            return View(model);
        }

        [HttpPost]
        public ActionResult Register(UserAccountVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please check the errors below");
                return View(model);
            }


            UserAccount us = new UserAccount();

            us.UniqueId = Guid.NewGuid();
            us.Fname = model.Fname;
            us.LastName = model.LastName;
            us.Email = model.Email;
            us.UserName = model.UserName;
            us.Password = Crypto.Hash(model.Password,"sha256");

            db.userAccounts.Add(us);
            db.SaveChanges();

            TempData["Success"] = "User: " + model.UserName + " has been successfuly created!";

            return RedirectToAction("Login", "UserAccount");
        }

        public ActionResult Login()
        {
            var model = new UserLoginModel();

            return View(model);
        }
    }
}