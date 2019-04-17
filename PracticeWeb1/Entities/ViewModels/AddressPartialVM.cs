using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities.ViewModels
{
    public class AddressPartialVM
    {
        [Required(ErrorMessage = "Country is Required")]
        [StringLength(50)]
        public string Country { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        [StringLength(1024)]
        public string Address { get; set; }
    }
}