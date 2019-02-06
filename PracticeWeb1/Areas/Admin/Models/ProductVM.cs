using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PracticeWeb1.Entities;

namespace PracticeWeb1.Areas.Admin.Models
{
    public class ProductVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual IEnumerable<SelectListItem> Categories { get; set; }
    }
}