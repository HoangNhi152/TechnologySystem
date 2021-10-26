using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechnologySystem.Models;

namespace TechnologySystem.ViewModels
{
    public class AssignCoursesViewModel
    {
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}