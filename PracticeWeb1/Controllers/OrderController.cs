using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PracticeWeb1.Custom_Authorization;
using PracticeWeb1.Entities;
using PracticeWeb1.Entities.ViewModels;
using PracticeWeb1.Models;

namespace PracticeWeb1.Controllers
{
    
    public class OrderController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Order
        [roleAuth]
        //This method should have been in admin area, will move it later
        public ActionResult Index()
        {
            var orders = db.orders.ToArray().OrderByDescending(x=>x.CreatedAt).Select(x => new OrderVM(x)).ToList();

            foreach (var order in orders)
            {
                order.UserName = db.userAccounts.Where(x => x.UniqueId.Equals(order.UniqueId)).FirstOrDefault().UserName;
            }

            return View(orders);
        }

        [HttpPost]
        public void PlaceOrder()
        {
            //Creating the order
            Order order = new Order();
            if (Session["uniqueId"] != null)
            {
                order.UniqueId = (Guid)Session["uniqueId"];
            }
            else
            {
                order.UniqueId = Guid.NewGuid();
            }

            order.CreatedAt = DateTime.Now;
            db.orders.Add(order);
            db.SaveChanges();

            var cart = (List<ItemVM>)Session["cart"];

            //Creating the OrderDetails of that order
            foreach(var item in cart)
            {
                OrderDetails orderDetails = new OrderDetails();
                orderDetails.OrderId = order.Id;
                orderDetails.ProductId = item.Product.Id;
                orderDetails.Quantity = item.Quantity;
                orderDetails.UniqueId = order.UniqueId;

                db.orderdetails.Add(orderDetails);
                db.SaveChanges();
            }

            Session["cart"] = null;
            TempData["orderSuccess"] = "Your Order has been placed.";

        }

        public ActionResult UserOrders()
        {
            var userId = new object();

            if (Session["uniqueId"] != null)
            {
                userId = (Guid)Session["uniqueId"];
            }
            else
            {
                return RedirectToAction("Login", "UserAccount", new { returnUrl = "/Order/UserOrders" });
            }
            //if(userId==null)
            //{
            //    RedirectToAction("Login", "UserAccount", new {returnUrl = "/Order/UserOrders" });
            //}

            var orderVMList = db.orders.ToArray().Where(x => x.UniqueId.Equals(userId)).OrderByDescending(x=>x.CreatedAt).Select(x=> new OrderVM(x)).ToList();

            foreach(var o in orderVMList)
            {
                o.UserName = Session["username"].ToString();
            }
            return View(orderVMList);
        }

        public ActionResult OrderDetailsPartial(int? id)
        {
            Order order = db.orders.Find(id);

            if (order == null)
            {
                ModelState.AddModelError("", "Order does not exist, please try again or contact administrator");
                return RedirectToAction("UserOrders", "Order");
            }

            var orderDetailsList = new List<OrderDetailsVM>();

            orderDetailsList = db.orderdetails.ToArray().Where(x => x.OrderId.Equals(id)).Select(x => new OrderDetailsVM(x)).ToList();

            decimal totalCost = 0m;
            foreach(var item in orderDetailsList)
            {
                item.Product = db.products.Where(x => x.Id == item.ProductId).SingleOrDefault();
                item.itemCost = item.Product.Price*item.Quantity;
                totalCost += item.itemCost;
            }

            TempData["totalCost"] = totalCost.ToString();

            return PartialView("_OrderDetailsPartial", orderDetailsList); ;
        }
    }
}