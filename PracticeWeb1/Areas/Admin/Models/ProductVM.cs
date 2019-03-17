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
        public ProductVM()
        {

        }
        public ProductVM(Product row)
        {
            Id = row.Id;
            Title = row.Title;
            Description = row.Description;
            CategoryId = row.CategoryId;
            Price = row.Price;
            ImageUrl = row.ImageUrl;
        }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get { return Description.Length < 25 ? Description : Description.Substring(0, 25) + "..."; } }
        [Display(Name ="Category")]
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}