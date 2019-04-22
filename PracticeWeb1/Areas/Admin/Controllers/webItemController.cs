using System;
using System.Collections;
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
            webItem.Type = getFileType(int.Parse(model.Type));

            db.webItems.Add(webItem);
            db.SaveChanges();

            if (addItemFile(webItem.Type, file))
            {
                webItem.itemUrl = file.FileName;
                TempData["success"] = "File Uploaded Successfully";
                db.SaveChanges();
            }
            else
            {
                return View("add-item", model);
            }

            return View("add-item");
        }

        public bool addItemFile(string type, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string ext = file.ContentType;

                if (ext != "video/mp4" &&
                    ext != "video/x-flv" &&
                    ext != "video/quicktime" &&
                    ext != "video/x-ms-wmv" &&
                    ext != "video/x-msvideo" &&
                    ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png" &&
                    ext != "audio/mpeg" &&
                    ext != "audio/mp3" &&
                    ext != "audio/mp4" &&
                    ext != "application/msword" &&
                    ext != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" &&
                    ext != "text/plain")
                {
                    ModelState.AddModelError("", "The video file is not compatible, please use one of the following formats: mp4, x-flv, quicktime, x-ms-wmv, x-msvideojpg, jpeg, jpg, gif, x-png, png, mpeg, mp4");
                    return false;
                }

                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "WebItems");
                var pathString2 = Path.Combine(originalDirectory.ToString(), "WebItems\\" + type);

                if (!Directory.Exists(pathString1))
                    Directory.CreateDirectory(pathString1);

                if (!Directory.Exists(pathString2))
                    Directory.CreateDirectory(pathString2);



                string fileTitle = file.FileName;

                var path = string.Format("{0}\\{1}", pathString2, fileTitle);

                file.SaveAs(path);

                return true;
            }
            else
            {
                return false;
            }
        }

        public string getFileType(int value)
        {
            string type = "";

            switch (value)
            {
                case 1:
                    type = "Video File";
                    break;
                case 2:
                    type = "Audio File";
                    break;
                case 3:
                    type = "Image File";
                    break;
                case 4:
                    type = "Text File";
                    break;
            }

            return type;
        }
    }
}