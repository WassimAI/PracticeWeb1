using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PracticeWeb1.Entities;

namespace PracticeWeb1.Areas.Admin.Models
{
    public class CategoryVM
    {
        public CategoryVM()
        {

        }

        public CategoryVM(Category row)
        {
            Id = row.Id;
            Title = row.Title;
            Description = row.Description;
            ImageUrl = row.ImageUrl;
        }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}