using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticeWeb1.Entities.ViewModels
{
    public class RoleVM
    {
        public RoleVM()
        {

        }

        public RoleVM(Role row)
        {
            Id = row.Id;
            Name = row.Name;
        }
        public int Id { get; set; }
        [Required]
        [StringLength(30,ErrorMessage ="Name cannot exceed 30 characters")]
        public string Name { get; set; }
    }
}