using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechnologySystem.Models;

namespace TechnologySystem.ViewModels
{
    public class ListUserViewModel
    {
        public Course Courses { get; set; }
        public ApplicationUser User{ get; set; }
}
}