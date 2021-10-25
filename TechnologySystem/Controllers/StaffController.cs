using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TechnologySystem.Models;
using TechnologySystem.Utils;
using TechnologySystem.ViewModels;

namespace TechnologySystem.Controllers
{
    [Authorize (Roles = Role.Staff)]
    public class StaffController : Controller
    {
        // GET: Admin/Account
        public async Task<ActionResult> Index(string searchString)
        {
            var traineeRole = await _context.Roles.SingleOrDefaultAsync(r => r.Name == Role.Trainee);
            var data = new UsersGroupViewModel
            {
                Trainees = await _context.Users
                        .Where(u => u.Roles.Any(r => r.RoleId == traineeRole.Id))
                        .ToListAsync()
            };
            
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                data.Trainees = data.Trainees.Where((c => !String.IsNullOrEmpty(c.FullName) && c.FullName.ToLower().Contains(searchString)
                                        || searchString.Contains(c.Age.ToString()))).ToList();
                return View(data);

            }
            return View(data);
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _context;

        public StaffController()
        {
            _context = new ApplicationDbContext();
        }

        public ApplicationSignInManager SignInManger
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,
                    FullName = model.FullName, Age = model.Age, DateofBirth = model.DateofBirth, Education = model.Education};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, Role.Trainee);
                    return RedirectToAction(nameof(Index));
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<ActionResult> Update(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new InfoViewModel()
            {
                User = user,
                Roles = new List<string>(await UserManager.GetRolesAsync(id))
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(InfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.User;
                var userinDb = await UserManager.FindByIdAsync(user.Id);

                if (userinDb == null)
                    return HttpNotFound();
                userinDb.FullName = user.FullName;
                userinDb.Age = user.Age;
                userinDb.DateofBirth = user.DateofBirth;
                userinDb.Email = user.Email;
                userinDb.UserName = user.Email;


                IdentityResult result = await UserManager.UpdateAsync(userinDb);

                // if (model.Specialty != null)
                // {
                //    var profile = await _context.TrainerProfiles.SingleOrDefaultAsync(p => p.UserId == userinDb.Id);
                //    profile.Specialty = model.Specialty;
                // }
                await _context.SaveChangesAsync();

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var user = UserManager.FindById(id);
            UserManager.Delete(user);
            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}