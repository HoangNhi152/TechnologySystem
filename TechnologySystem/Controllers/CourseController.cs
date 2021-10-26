using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        private UserManager<ApplicationUser> _userManager;
        public CourseController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
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

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null) return HttpNotFound();

            var course = _context.Courses
                //.Include(t => t.Category)
                .SingleOrDefault(t => t.Id == id);

            if (course == null) return HttpNotFound();

            return View(course);
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

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult ShowTrainers(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var members = _context.AssignCourses
                //.Include(t => t.User)
                .Where(t => t.CourseId == id)
                .Select(t => t.User);
            var trainer = new List<ApplicationUser>();       // Init List Users to Add Course

            foreach (var user in members)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("Trainer"))
                {
                    trainer.Add(user);
                }
            }
            ViewBag.CourseId = id;
            return View(trainer);
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AddTrainers(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            if (_context.Courses.SingleOrDefault(t => t.Id == id) == null)
                return HttpNotFound();

            var usersInDb = _context.Users.ToList();      // User trong Db

            var usersInTeam = _context.AssignCourses         // User trong Team
                //.Include(t => t.User)
                .Where(t => t.CourseId == id)
                .Select(t => t.User)
                .ToList();

            var usersToAdd = new List<ApplicationUser>();       // Init List Users to Add Team

            foreach (var user in usersInDb)
            {
                if (!usersInTeam.Contains(user) &&
                    _userManager.GetRoles(user.Id)[0].Equals("Trainer"))
                {
                    usersToAdd.Add(user);
                }
            }

            var viewModel = new AssignCoursesViewModel
            {
                CourseId = (int)id,
                Users = usersToAdd
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult AddTrainers(AssignCourse model)
        {
            var courseUser = new AssignCourse
            {
                CourseId = model.CourseId,
                UserId = model.UserId
            };

            _context.AssignCourses.Add(courseUser);
            _context.SaveChanges();

            return RedirectToAction("ShowTrainers", new { id = model.CourseId });
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult RemoveTrainers(int id, string userId)
        {
            var courseUserToRemove = _context.AssignCourses
                .SingleOrDefault(t => t.CourseId == id && t.UserId == userId);

            if (courseUserToRemove == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            _context.AssignCourses.Remove(courseUserToRemove);
            _context.SaveChanges();
            return RedirectToAction("ShowTrainers", new { id = id });
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult ShowTrainees(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var members = _context.AssignCourses
                //.Include(t => t.User)
                .Where(t => t.CourseId == id)
                .Select(t => t.User);
            var trainee = new List<ApplicationUser>();       // Init List Users to Add Course

            foreach (var user in members)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("Trainee"))
                {
                    trainee.Add(user);
                }
            }
            ViewBag.CourseId = id;
            return View(trainee);
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult AddTrainees(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            if (_context.Courses.SingleOrDefault(t => t.Id == id) == null)
                return HttpNotFound();

            var usersInDb = _context.Users.ToList();      // User trong Db

            var usersInTeam = _context.AssignCourses         // User trong Team
                //.Include(t => t.User)
                .Where(t => t.CourseId == id)
                .Select(t => t.User)
                .ToList();

            var usersToAdd = new List<ApplicationUser>();       // Init List Users to Add Team

            foreach (var user in usersInDb)
            {
                if (!usersInTeam.Contains(user) &&
                    _userManager.GetRoles(user.Id)[0].Equals("Trainee"))
                {
                    usersToAdd.Add(user);
                }
            }

            var viewModel = new AssignCoursesViewModel
            {
                CourseId = (int)id,
                Users = usersToAdd
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult AddTrainees(AssignCourse model)
        {
            var courseUser = new AssignCourse
            {
                CourseId = model.CourseId,
                UserId = model.UserId
            };

            _context.AssignCourses.Add(courseUser);
            _context.SaveChanges();

            return RedirectToAction("ShowTrainees", new { id = model.CourseId });
        }
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult RemoveTrainees(int id, string userId)
        {
            var courseUserToRemove = _context.AssignCourses
                .SingleOrDefault(t => t.CourseId == id && t.UserId == userId);

            if (courseUserToRemove == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            _context.AssignCourses.Remove(courseUserToRemove);
            _context.SaveChanges();

            return RedirectToAction("ShowTrainees", new { id = id });
        }

    }
}