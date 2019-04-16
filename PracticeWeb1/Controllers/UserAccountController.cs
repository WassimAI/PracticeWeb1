using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PracticeWeb1.Custom_Authorization;
using PracticeWeb1.Entities;
using PracticeWeb1.Entities.ViewModels;
using PracticeWeb1.Models;

namespace PracticeWeb1.Controllers
{
    public class UserAccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserAccount
        [roleAuth]
        public ActionResult Index()
        {
            var userVMList = new List<UserAccountVM>();

            userVMList = db.userAccounts.ToArray().Select(x => new UserAccountVM(x)).ToList();

            return View(userVMList);
        }

        public ActionResult Register(string returnUrl)
        {
            var model = new UserAccountVM();
            model.returnUrl = returnUrl;

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
            us.Country = model.Country;
            us.Address = model.Address;

            db.userAccounts.Add(us);
            db.SaveChanges();

            TempData["Success"] = "User:<strong> " + model.UserName + " </strong>has been successfuly created!";

            string url = (!string.IsNullOrEmpty(model.returnUrl) && Url.IsLocalUrl(model.returnUrl)) ? model.returnUrl : "/UserAccount/Login";

            if(url == model.returnUrl)
            {
                #region loginAutomatically
                Session["username"] = us.UserName;
                Session["email"] = us.Email;
                Session["uniqueId"] = us.UniqueId;

                Session.Add("uniqueId", us.UniqueId);

                int roleId = 0;
                string role = "";

                if (db.userRoles.Where(x => x.UniqueId == us.UniqueId).FirstOrDefault() != null)
                {
                    roleId = db.userRoles.Where(x => x.UniqueId == us.UniqueId).FirstOrDefault().RoleId;
                    role = db.tblRoles.Where(x => x.Id == roleId).FirstOrDefault().Name;
                }

                if (role == "admin")
                {
                    Session["role"] = role;
                }
                else
                {
                    Session["role"] = null;
                }
                #endregion
            }
            return Redirect(url);
            //return RedirectToAction("Login", "UserAccount");
        }

        public ActionResult Login(string returnUrl)
        {
            var model = new UserLoginModel();

            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Errors Below.");
                return View(model);
            }

            var hashedPassword = Crypto.Hash(model.Password);


            UserAccount user = db.userAccounts.Where(x => x.Email == model.Email && x.Password == hashedPassword).FirstOrDefault();

            if(user == null)
            {
                ModelState.AddModelError("", "Invalid User Name or Password");
                return View(model);
            }

            int roleId = 0;
            string role = "";

            if(db.userRoles.Where(x=>x.UniqueId==user.UniqueId).FirstOrDefault() != null)
            {
                roleId = db.userRoles.Where(x => x.UniqueId == user.UniqueId).FirstOrDefault().RoleId;
                role = db.tblRoles.Where(x => x.Id == roleId).FirstOrDefault().Name;
            }


            if (role == "admin")
            {
                Session["role"] = role;
            }
            else
            {
                Session["role"] = null;
            }

            Session["username"] = user.UserName;
            Session["email"] = user.Email;
            Session["uniqueId"] = user.UniqueId;

            Session.Add("uniqueId", user.UniqueId);

            string url = (!String.IsNullOrEmpty(model.ReturnUrl)) && Url.IsLocalUrl(model.ReturnUrl) ? model.ReturnUrl : "/Home/Index";

            return Redirect(url);
        }

        [HttpPost]
        public void Logoff()
        {
            Session.Clear();
        }
    }
}
