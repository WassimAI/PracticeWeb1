using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities
{
    [Table("UserRole")]
    public class UserRole
    {
        [Key, Column(Order = 0)]
        public Guid UniqueId { get; set; }
        [Key, Column(Order = 1)]
        public int RoleId { get; set; }

        [ForeignKey("UniqueId")]
        public virtual UserAccount user { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role role { get; set; }
    }
}