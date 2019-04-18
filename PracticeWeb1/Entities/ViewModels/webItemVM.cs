using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities.ViewModels
{
    public class webItemVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="The title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "The type is required")]
        public string Type { get; set; }
        public string itemUrl { get; set; }
    }
}