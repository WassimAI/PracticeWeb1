using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities.ViewModels
{
    public class OrderVM
    {
        public OrderVM()
        {

        }

        public OrderVM(Order row)
        {
            Id = row.Id;
            UniqueId = row.UniqueId;
            CreatedAt = row.CreatedAt;
        }
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        [Display(Name ="Created At")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }
}