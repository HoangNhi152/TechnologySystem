using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechnologySystem.Models;

namespace TechnologySystem.ViewModels
{
    public class CourseCategories
    {
        public Course Course { get; set; }
        public List<Category> Categories { get; set; }
    }
}