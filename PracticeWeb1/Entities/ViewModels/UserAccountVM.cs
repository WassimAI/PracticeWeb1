using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities.ViewModels
{
    public class UserAccountVM
    {
        public UserAccountVM()
        {

        }

        public UserAccountVM(UserAccount row)
        {
            UniqueId = row.UniqueId;
            Fname = row.Fname;
            LastName = row.LastName;
            Email = row.Email;
            Password = row.Password;
            UserName = row.UserName;
        }

        [Display(Name ="User ID")]
        public Guid UniqueId { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name ="First Name")]
        public string Fname { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$", ErrorMessage ="Invalid Email Address.")]
        public string Email { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [StringLength(16, ErrorMessage ="Password cannot be more than 16 characters.")]
        public string Password { get; set; }
        [Required]
        [StringLength(16)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage ="Passwords do not Match")]
        public string ConfirmPassword { get; set; }
        public string FullName { get { return Fname + " " + LastName; } }
    }
}