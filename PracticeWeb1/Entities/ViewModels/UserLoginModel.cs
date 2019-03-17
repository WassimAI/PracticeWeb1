using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities.ViewModels
{
    public class UserLoginModel
    {
        [Required]
        [StringLength(50)]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$", ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "Password cannot be more than 16 characters.")]
        public string Password { get; set; }
        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
}