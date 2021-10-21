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

        public ActionResult Edit(int id)
        {
            var Course = _context.Courses.SingleOrDefault(t => t.Id == id);
            return View(Course);
        }

        [HttpPost]
        public ActionResult Edit(Course newCourse)
        {
            if(ModelState.IsValid)
            {
                var oldCourse = _context.Courses.SingleOrDefault(c => c.Id == newCourse.Id);
                oldCourse.CourseName = newCourse.CourseName;
                oldCourse.Description = newCourse.Description;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var course = _context.Courses.SingleOrDefault(c => c.Id == id);
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}