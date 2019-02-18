using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using PracticeWeb1.Areas.Admin.Models;
using PracticeWeb1.Entities;
using PracticeWeb1.Models;

namespace PracticeWeb1.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Category
        public ActionResult Index(string Search, string Categories, string sortBy, int? page)
        {
            ViewBag.NameSort = string.IsNullOrEmpty(sortBy) ? "NameDesc" : "";

            var categoriesList = new List<string>();

            categoriesList.AddRange(db.categories.Select(x => x.Title).Distinct());//Distinct is to avoid repetition of some titles

            ViewBag.Categories = new SelectList(categoriesList);

            var categories = db.categories.AsQueryable();

            if (!string.IsNullOrEmpty(Search))
            {
                categories = categories.Where(x => x.Title.Contains(Search));
            }

            if (!string.IsNullOrEmpty(Categories))
            {
                categories = categories.Where(x => x.Title.Equals(Categories));
            }

            if (sortBy == "NameDesc")
                return View(categories.OrderByDescending(x => x.Title).ToPagedList(page ?? 1, 3));
            else
                return View(categories.OrderBy(x => x.Title).ToPagedList(page ?? 1, 3));
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.categories.Find(id);

            var model = new CategoryVM(category);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            var model = new CategoryVM();

            return View(model);
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryVM model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (db.categories.Any(x => x.Title == model.Title))
            {
                ModelState.AddModelError("", "The Category Name already exists.");
                return View(model);
            }

            Category category = new Category();

            category.Title = model.Title;
            category.Description = model.Description;


            db.categories.Add(category);
            db.SaveChanges();

            if (!addImage(category.Id, file))
            {
                ModelState.AddModelError("", "Image Not uploaded - wrong image extension.");
                return View(model);
            }

            if (addImage(category.Id, file))
            {
                category.ImageUrl = file.FileName;
                db.SaveChanges();
            }

            return RedirectToAction("Index");

        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.categories.Find(id);

            var model = new CategoryVM(category);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryVM model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Category category = db.categories.Where(x => x.Id.Equals(model.Id)).FirstOrDefault();

            category.Title = model.Title;
            category.Description = model.Description;



            if (file != null)
            {
                DeleteImage(category.Id, category.ImageUrl);
                if (addImage(category.Id, file))
                {
                    category.ImageUrl = file.FileName;
                }
                else
                {
                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                    return View(model);
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.categories.Find(id);

            var model = new CategoryVM(category);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.categories.Find(id);

            if (db.products.Any(x => x.CategoryId.Equals(category.Id)))
            {
                TempData["AlertMsg"] = "You cannot delete this category as it contains products, please delete all products of this category first.";
                var model = new CategoryVM(category);

                return View("Delete", model);
            }

            db.categories.Remove(category);
            db.SaveChanges();

            //Delete category folder
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string pathString = Path.Combine(originalDirectory.ToString(), "Categories\\" + id.ToString());

            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);

            return RedirectToAction("Index");
        }

        public int DeleteMany(int[] ids)
        {

            List<Category> listToDelete = db.categories.Where(x => ids.Contains(x.Id)).ToList();


            foreach (var item in listToDelete)
            {
                if (db.products.Any(x => x.CategoryId.Equals(item.Id)))
                {
                    return -1;
                }

                db.categories.Remove(item);

                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                string pathString = Path.Combine(originalDirectory.ToString(), "Categories\\" + item.Id.ToString());

                if (Directory.Exists(pathString))
                    Directory.Delete(pathString, true);

                db.SaveChanges();

            }

            return 1;

            //DeleteCategoriesFolder(ids);



        }

        public bool addImage(int id, HttpPostedFileBase file)
        {
            //Add new image
            // Create necessary directories
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Categories");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Categories\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Categories\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Categories\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Categories\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            if (file != null && file.ContentLength > 0)
            {
                // Get file extension
                string ext = file.ContentType.ToLower();

                // Verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    //ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                    return false;
                }

                // Init image name
                string imageName = file.FileName;

                // Save image name to product Object
                //category.ImageUrl = imageName;

                // Set original and thumb image paths
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                // Save original
                file.SaveAs(path);

                // Create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(150, 150);
                img.Save(path2);


            }

            return true;

        }

        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Categories/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Categories/" + id.ToString() + "/Gallery/Thumbs/" + imageName);
            string fullPath3 = Request.MapPath("~/Images/Uploads/Categories/" + id.ToString() + "/" + imageName);
            string fullPath4 = Request.MapPath("~/Images/Uploads/Categories/" + id.ToString() + "/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);

            if (System.IO.File.Exists(fullPath3))
                System.IO.File.Delete(fullPath3);

            if (System.IO.File.Exists(fullPath4))
                System.IO.File.Delete(fullPath4);
        }

        public void DeleteCategoriesFolder(int[] ids)
        {
            foreach (int id in ids)
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                string pathString = Path.Combine(originalDirectory.ToString(), "Categories\\" + id.ToString());

                if (Directory.Exists(pathString))
                    Directory.Delete(pathString, true);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
