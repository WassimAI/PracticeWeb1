using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid? UniqueId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("UniqueId")]
        public virtual UserAccount user { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order order { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product product { get; set; }
    }
}