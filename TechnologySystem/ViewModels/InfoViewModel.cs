using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TechnologySystem.Models;

namespace TechnologySystem.ViewModels
{
    public class InfoViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
        public string Specialty { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; set; }

        public string Education { get; set; }
    }
}