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
        public ActionResult Index()
        {
            var orders = db.orders.ToArray().Select(x => new OrderVM(x)).ToList();

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

        }

        public ActionResult UserOrders()
        {
            var userId = (Guid)Session["uniqueId"];
            //if(userId==null)
            //{
            //    RedirectToAction("Login", "UserAccount", new {returnUrl = "/Order/UserOrders" });
            //}

            var orderVMList = db.orders.ToArray().Where(x => x.UniqueId.Equals(userId)).Select(x=> new OrderVM(x)).ToList();

            foreach(var o in orderVMList)
            {
                o.UserName = Session["username"].ToString();
            }
            return View(orderVMList);
        }
    }
}