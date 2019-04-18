using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PracticeWeb1.Entities;
using PracticeWeb1.Entities.ViewModels;
using PracticeWeb1.Models;

namespace PracticeWeb1.Areas.Admin.Controllers
{
    public class webItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/webItem
        [ActionName("add-item")]
        public ActionResult addItem()
        {
            webItemVM model = new webItemVM();

            return View(model);
        }

        [HttpPost]
        public ActionResult addItem(webItemVM model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            webItem webItem = new webItem();

            webItem.Title = model.Title;
            webItem.Description = model.Description;
            webItem.Type = model.Type;

            db.webItems.Add(webItem);
            db.SaveChanges();

            if(addItemFile(webItem.Title, file))
            {
                webItem.itemUrl = file.FileName;
                db.SaveChanges();
            }

            TempData["success"] = "File Uploaded Successfully";

            return View();
        }

        public bool addItemFile(string title, HttpPostedFileBase file)
        {
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "WebItems");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "WebItems\\" + title);

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if(file!=null && file.ContentLength > 0)
            {
                string ext = file.ContentType;

                if(ext != "video/mp4")
                {
                    ModelState.AddModelError("", "The video file is not compatible, please use .mp4 format");
                    return false;
                }

                string fileTitle = file.FileName;

                var path = string.Format("{0}\\{1}", pathString2, fileTitle);

                file.SaveAs(path);

                return true;
            } else
            {
                return false;
            }
        }
    }
}