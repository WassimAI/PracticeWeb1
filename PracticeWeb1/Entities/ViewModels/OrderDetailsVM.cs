using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities.ViewModels
{
    public class OrderDetailsVM
    {
        public OrderDetailsVM()
        {

        }

        public OrderDetailsVM(OrderDetails row)
        {
            Id = row.Id;
            OrderId = row.OrderId;
            UniqueId = row.UniqueId;
            ProductId = row.ProductId;
            Quantity = row.Quantity;
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid? UniqueId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }

        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}