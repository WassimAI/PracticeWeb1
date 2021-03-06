﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PracticeWeb1.Areas.Admin.Models;
using PracticeWeb1.Entities;
using PracticeWeb1.Entities.ViewModels;
using PracticeWeb1.Models;

namespace PracticeWeb1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Landing()
        {
            webItemVM model = new webItemVM();

            return View(model);
        }
        public ActionResult Index()
        {
            var categories = new List<Category>();

            categories = db.categories.ToList();

            return View(categories);
        }

        public ActionResult AllProducts(int? id)
        {
            int qty = 0;
            decimal totalPrice = 0m;

            var listOfProducts = new List<ProductVM>();

            //Surprisingly this only worked when I used == instead of .Equal() method
            listOfProducts = db.products.Where(x => x.CategoryId == id).ToArray().Select(x => new ProductVM(x)).ToList();

            ViewBag.PageTitle = "AllProducts";

            if (Session["cart"] != null)
            {
                var itemList = (List<ItemVM>)Session["cart"];
                foreach (var item in itemList)
                {
                    qty += item.Quantity;
                    totalPrice += item.Product.Price * item.Quantity;
                }

                ViewBag.qty = qty;
                ViewBag.totalPrice = totalPrice;
            }


            return View(listOfProducts);
        }

        public ActionResult ProductDetails(int? id)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            Product product = db.products.Find(id);

            ViewBag.categoryId = product.CategoryId;

            var model = new ProductVM(product);

            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                               .Select(fn => Path.GetFileName(fn));

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}