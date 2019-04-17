using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PracticeWeb1.Areas.Admin.Models;

namespace PracticeWeb1.Areas.Admin.Controllers
{
    public class MainController : Controller
    {
        public ActionResult UploadVideo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadVideo(HttpPostedFileBase file)
        {
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Videos");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Videos\\" + file.FileName);

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (file != null && file.ContentLength > 0)
            {
                // Get file extension
                //string ext = file.ContentType.ToLower();
                string ext = file.ContentType;

                // Verify extension
                if (ext != "video/mp4" //&&
                                       //ext != ".MP4" &&
                                       //ext != ".WAV" &&
                                       //ext != ".AVI" &&
                                       //ext != ".WMV")
                    )
                {
                    ModelState.AddModelError("", "The Video extension is not correct.");
                    return View();
                }



                // Set original and thumb image paths
                var path = string.Format("{0}\\{1}", pathString2, file.FileName);

                // Save original
                file.SaveAs(path);

            }

            TempData["uploadSuccess"] = "Video Uploaded Successfully!";

            return View();
        }
    }
}