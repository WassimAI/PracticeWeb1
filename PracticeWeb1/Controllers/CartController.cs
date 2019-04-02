﻿using System;
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

        [HttpPost]
        public ActionResult ClearCart()
        {
            var model = new CartDetailsVM()
            {
                Items = new List<ItemVM>(),
                Qty = 0,
                TotalPrice = 0
            };

            Session["cart"] = null;

            return PartialView("_CartDetailsPartial", model);
        }
    }
}