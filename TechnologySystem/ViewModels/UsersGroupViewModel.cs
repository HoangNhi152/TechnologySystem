using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechnologySystem.Models;

namespace TechnologySystem.ViewModels
{
    public class UsersGroupViewModel
    {
        public List<ApplicationUser> Staffs { get; set; }
        public List<ApplicationUser> Trainers { get; set; }
        public List<ApplicationUser> Trainees { get; set; }
    }
}