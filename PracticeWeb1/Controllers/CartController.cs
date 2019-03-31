using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PracticeWeb1.Entities;
using PracticeWeb1.Entities.ViewModels;
using PracticeWeb1.Models;

namespace PracticeWeb1.Controllers
{
    public class CartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        //POST: Cart/AddToCart/1
        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            var model = new CartVM();
            decimal totalPrice = 0m;
            int qty = 0;

            //Create the items list
            List<ItemVM> cart = Session["cart"] as List<ItemVM> ?? new List<ItemVM>();

            //Get Product
            Product product = db.products.Find(productId);

            if (product == null)
            {
                ModelState.AddModelError("", "Something went wrong, please try again.");
            }

            if (Session["cart"] == null)
            {
                var item = new ItemVM
                {
                    Product = product,
                    Quantity = 1
                };

                cart.Add(item);

                //Updating Sesssion
                Session["cart"] = cart;
            }
            else
            {
                var itemInCart = cart.FirstOrDefault(x => x.Product.Id.Equals(productId));

                if(itemInCart == null)
                {
                    var item = new ItemVM
                    {
                        Product = product,
                        Quantity = 1
                    };

                    cart.Add(item);

                }
                else
                {
                    itemInCart.Quantity++;
                }

                Session["cart"] = cart;
            }

            foreach(var item in cart)
            {
                qty += item.Quantity;
                totalPrice += item.Product.Price * item.Quantity;
            }

            model.Qty = qty;
            model.TotalPrice = totalPrice;

            return PartialView("_AddToCartPartial", model);
        }

        [HttpPost]
        public ActionResult ClearCart()
        {
            var model = new CartVM();

            Session["cart"] = null;

            return PartialView("_AddToCartPartial", model);
        }
    }
}