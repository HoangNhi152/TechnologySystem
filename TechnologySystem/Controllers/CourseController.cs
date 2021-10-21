using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnologySystem.Models;

namespace TechnologySystem.Controllers
{
    public class CourseController : Controller
    {
        private ApplicationDbContext _context;
        public CourseController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index(string searchString)
        {
            var courses = _context.Courses.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                courses = courses.Where(c => c.CourseName.ToLower().Contains(searchString)).ToList();
            }

            return View(courses);
        }

    }
}