using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PracticeWeb1.Custom_Authorization;
using PracticeWeb1.Entities.ViewModels;
using PracticeWeb1.Models;

namespace PracticeWeb1.Controllers
{
    [roleAuth]
    public class OrderController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Order
        public ActionResult Index()
        {
            var orders = db.orders.ToArray().Select(x => new OrderVM(x)).ToList();

            foreach (var order in orders)
            {
                order.UserName = db.userAccounts.Where(x => x.UniqueId.Equals(order.UniqueId)).FirstOrDefault().UserName;
            }

            return View(orders);
        }

        public ActionResult PlaceOrder()
        {
            return View();
        }
    }
}