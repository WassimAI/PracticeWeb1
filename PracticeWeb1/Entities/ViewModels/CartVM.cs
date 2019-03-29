using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities.ViewModels
{
    public class CartVM
    {
        public CartVM()
        {
            TotalPrice = 0m;
        }
        public int Qty { get; set; }
        public decimal TotalPrice { get; set; }
    }
}