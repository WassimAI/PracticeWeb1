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
            var model = new CartDetailsVM();

            var listofItems = new List<ItemVM>();

            if (Session["cart"] != null)
            {
                var cart = (List<ItemVM>)Session["cart"];

                foreach (var item in cart)
                {
                    model.Qty += item.Quantity;
                    model.TotalPrice += item.Product.Price * item.Quantity;
                    listofItems.Add(item);
                }
            }

            ViewBag.CartPage = "cartIndex";

            model.Items = listofItems;

            return View(model);
        }

        //POST: Cart/AddToCart/1
        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            //var model = new CartVM();
            var model = new CartDetailsVM();
            decimal totalPrice = 0m;
            int qty = 0;

            //Create the items list
            List<ItemVM> cart = Session["cart"] as List<ItemVM> ?? new List<ItemVM>();

            //Initialize the item
            var item = new ItemVM();
            var listOfItems = new List<ItemVM>();

            //Get Product
            Product product = db.products.Find(productId);

            if (product == null)
            {
                ModelState.AddModelError("", "Something went wrong, please try again.");
            }

            if (Session["cart"] == null)
            {
                item = new ItemVM
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

                if (itemInCart == null)
                {
                    item = new ItemVM
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

            foreach (var cartItem in cart)
            {
                qty += cartItem.Quantity;
                totalPrice += cartItem.Product.Price * cartItem.Quantity;
                listOfItems.Add(cartItem);
            }

            model.Qty = qty;
            model.TotalPrice = totalPrice;
            model.Items = listOfItems;

            return PartialView("_CartDetailsPartial", model);
        }

        public ActionResult CartPartial()
        {
            CartVM model = new CartVM();

            int qty = 0;
            decimal price = 0m;

            if (Session["cart"] != null)
            {
                //This is to initialize a list from the session, which already has an items list
                var list = (List<ItemVM>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Product.Price;
                }

                model.Qty = qty;
                model.TotalPrice = price;

            }
            else
            {
                // Or set qty and price to 0
                model.Qty = 0;
                model.TotalPrice = 0m;
            }

            // Return partial view with model
            return PartialView("_CartPartial", model);
        }

        public JsonResult incProduct(int id)
        {
            var model = new CartDetailsVM();
            var listofItesm = new List<ItemVM>();

            int qty = 0;
            decimal price = 0m;
            decimal itemTotalPrice = 0m;

            List<ItemVM> cart = Session["cart"] as List<ItemVM>;

            ItemVM itemToInc = cart.FirstOrDefault(x => x.Product.Id.Equals(id));

            itemToInc.Quantity++;

            foreach(var item in cart)
            {
                qty += item.Quantity;
                price += item.Product.Price * item.Quantity;
            }

            itemTotalPrice = itemToInc.Product.Price * itemToInc.Quantity;

            var result = new { totalQuantity = qty, totalPrice = price,
                quantity = itemToInc.Quantity, itemPrice = itemToInc.Product.Price,
                itemTotalPrice = Math.Round(itemTotalPrice, 2)
            };

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult decProduct(int id)
        {
            var model = new CartDetailsVM();
            var listofItesm = new List<ItemVM>();

            int qty = 0;
            decimal price = 0m;
            decimal itemTotalPrice = 0m;
            bool isLastItem = false;
            bool isCartEmpty = false;

            List<ItemVM> cart = Session["cart"] as List<ItemVM>;

            ItemVM itemToInc = cart.FirstOrDefault(x => x.Product.Id.Equals(id));

            if (itemToInc.Quantity > 1)
            {
                itemToInc.Quantity--;
            }
            else
            {
                itemToInc.Quantity = 0;
                cart.Remove(itemToInc);
                isLastItem = true;
            }

            if (cart.Count() == 0)
            {
                isCartEmpty = true;
            }

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Product.Price * item.Quantity;
            }

            itemTotalPrice = itemToInc.Product.Price * itemToInc.Quantity;

            var result = new
            {
                totalQuantity = qty,
                totalPrice = price,
                quantity = itemToInc.Quantity,
                itemPrice = itemToInc.Product.Price,
                itemTotalPrice = Math.Round(itemTotalPrice, 2),
                isLastItem = isLastItem,
                isCartEmpty = isCartEmpty
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PlaceOrder()
        {
            var model = new CartDetailsVM();

            var listofItems = new List<ItemVM>();

            if (Session["cart"] != null)
            {
                var cart = (List<ItemVM>)Session["cart"];

                foreach (var item in cart)
                {
                    model.Qty += item.Quantity;
                    model.TotalPrice += item.Product.Price * item.Quantity;
                    listofItems.Add(item);
                }
            }

            model.Items = listofItems;

            return View(model);
        }

        public JsonResult checkAddress(string userName)
        {
            //Get user
            var user = db.userAccounts.Where(x=>x.UserName.Equals(userName)).FirstOrDefault();
            string response="";

            if(user == null)
            {
                ModelState.AddModelError("", "Sorry, the user account seems to be missing, please try to login again");
                response = "error";
                var result = new {
                    response = response
                };
                return Json(result , JsonRequestBehavior.AllowGet);
            }

            if(user.Country == null || user.Address == null)
            {
                response = "No";
                var result = new
                {
                    response = response
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            } else
            {
                response = "Yes";
                var result = new
                {
                    response = response
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public void ClearCart()
        {
            Session["cart"] = null;
        }
    }
}