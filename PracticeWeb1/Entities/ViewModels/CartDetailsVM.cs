using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities.ViewModels
{
    public class CartDetailsVM
    {
        public CartDetailsVM()
        {
            TotalPrice = 0m;
            Qty = 0;
        }
        public IEnumerable<ItemVM> Items { get; set; }
        public int Qty { get; set; }
        public decimal TotalPrice { get; set; }
    }
}