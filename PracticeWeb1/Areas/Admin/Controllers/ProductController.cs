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
using PracticeWeb1.Areas.Admin.Models;
using PracticeWeb1.Entities;
using PracticeWeb1.Models;
using PagedList;
using PagedList.Mvc;
using PracticeWeb1.Custom_Authorization;

namespace PracticeWeb1.Areas.Admin.Controllers
{
    [roleAuth]
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Product
        public ActionResult Index(string Search, string Categories, int? Page, string sortBy)
        {
            ViewBag.NameSort = string.IsNullOrEmpty(sortBy) ? "NameDesc" : "";

            var categoriesList = new List<string>();

            categoriesList.AddRange(db.categories.Select(x => x.Title).Distinct());//Distinct is to avoid repetition of some titles

            ViewBag.Categories = new SelectList(categoriesList);

            var products = db.products.AsQueryable();

            //List<ProductVM> productVMList;

            if (!string.IsNullOrEmpty(Search))
            {
                products = products.Where(x => x.Title.Contains(Search));
            }

            if (!string.IsNullOrEmpty(Categories))
            {
                int CatId = db.categories.FirstOrDefault(x => x.Title.Equals(Categories)).Id;
                products = products.Where(x => x.CategoryId == CatId);
            }

            if (sortBy == "NameDesc")
                return View(products.OrderByDescending(x => x.Title).ToPagedList(Page ?? 1, 10));
            else
                return View(products.OrderBy(x => x.Title).ToPagedList(Page ?? 1, 10));
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            ProductVM model = new ProductVM()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                CategoryName = db.categories.FirstOrDefault(x => x.Id.Equals(product.CategoryId)).Title,
                Price = product.Price,
                ImageUrl = product.ImageUrl
            };

            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                               .Select(fn => Path.GetFileName(fn));


            return View(model);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            var model = new ProductVM();

            ViewBag.CategoryId = new SelectList(db.categories, "Id", "Title");

            return View(model);
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVM model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(db.categories, "Id", "Title");
                return View(model);
            }

            if (db.products.Any(x => x.Title == model.Title))
            {
                ViewBag.CategoryId = new SelectList(db.categories, "Id", "Title");
                ModelState.AddModelError("", "That product name is taken!");
                return View(model);
            }

            Product product = new Product();

            product.Title = model.Title;
            product.Description = model.Description;
            product.Price = model.Price;

            //Category cat = db.categories.FirstOrDefault(x => x.Id == model.CategoryId); This to be used for category name later

            product.CategoryId = model.CategoryId;

            db.products.Add(product);
            db.SaveChanges();

            //Get the Product Id to use in Image Upload
            int id = product.Id;

            if (!addImage(id, file))
            {
                ViewBag.CategoryId = new SelectList(db.categories, "Id", "Title");
                ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                return View(model);
            }

            if (addImage(id, file))
            {
                product.ImageUrl = file.FileName;
                db.SaveChanges();
            }



            return RedirectToAction("Index");
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryId = new SelectList(db.categories, "Id", "Title", product.CategoryId);

            ProductVM model = new ProductVM()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId
            };

            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                               .Select(fn => Path.GetFileName(fn));

            return View(model);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductVM model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var productToChange = db.products.Where(x => x.Id.Equals(model.Id)).FirstOrDefault();

            productToChange.Title = model.Title;
            productToChange.Description = model.Description;
            productToChange.Price = model.Price;
            productToChange.CategoryId = model.CategoryId;

            db.SaveChanges();

            if (file != null)
            {
                DeleteImage(productToChange.Id, productToChange.ImageUrl);
                if (addImage(productToChange.Id, file))
                {
                    productToChange.ImageUrl = file.FileName;
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                    ViewBag.CategoryId = new SelectList(db.categories, "Id", "Title", model.CategoryId);
                    return View(model);
                }
            }          

            return RedirectToAction("Index");
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();

            //Delete product folder
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());

            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);

            return RedirectToAction("Index");
        }

        // /Admin/Product/
        [HttpPost]
        public void DeleteMany(int[] ids)
        {

            List<Product> listToDelete = db.products.Where(x => ids.Contains(x.Id)).ToList();


            foreach (var item in listToDelete)
            {
                db.products.Remove(item);
            }

            DeleteProductsFolder(ids);

            db.SaveChanges();

        }

        public bool addImage(int id, HttpPostedFileBase file)
        {
            //Add new image
            // Create necessary directories
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

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
                    ViewBag.CategoryId = new SelectList(db.categories, "Id", "Title");
                    ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                    return false;
                }

                // Init image name
                string imageName = file.FileName;

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

        public void DeleteProductsFolder(int[] ids)
        {
            foreach (int id in ids)
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                string pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());

                if (Directory.Exists(pathString))
                    Directory.Delete(pathString, true);
            }

        }

        // POST: Admin/Product/SaveGalleryImages
        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            // Loop through files
            foreach (string fileName in Request.Files)
            {
                // Init the file
                HttpPostedFileBase file = Request.Files[fileName];

                // Check it's not null
                if (file != null && file.ContentLength > 0)
                {
                    // Set directory paths
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                    string pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

                    // Set image paths
                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);

                    // Save original and thumb

                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(150, 150);
                    img.Save(path2);
                }

            }

        }

        // POST: Admin/Product/DeleteImage
        [HttpPost]
        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/Thumbs/" + imageName);
            string fullPath3 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/" + imageName);
            string fullPath4 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);

            if (System.IO.File.Exists(fullPath3))
                System.IO.File.Delete(fullPath3);

            if (System.IO.File.Exists(fullPath4))
                System.IO.File.Delete(fullPath4);
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
