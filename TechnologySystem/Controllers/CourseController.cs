using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnologySystem.Models;
using TechnologySystem.ViewModels;

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

        public ActionResult Create()
        {
            var model = new ViewModels.CourseCategories()
            {
                Categories = _context.Categories.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CourseCategories courseCategories, int Id)
        {
            if(ModelState.IsValid)
            {
                var newCourse = new Course
                {
                    CourseName = courseCategories.Course.CourseName,
                    Description = courseCategories.Course.Description,
                    CategoryId = Id
                };
                _context.Courses.Add(newCourse);
            _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}