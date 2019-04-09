using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UniqueId")]
        public virtual UserAccount User { get; set; }
    }
}