using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
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
    [Authorize(Roles = Role.Trainer)]
    public class TrainerController : Controller
    {
        // GET: Trainer
        private ApplicationUserManager _userManager;
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
        private ApplicationDbContext _context;
        public TrainerController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var trainer = _context.Users.SingleOrDefault(u => u.Id.Equals(userId));

            if (trainer == null) return HttpNotFound();
            return View(trainer);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
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
        [HttpPost]
        public async Task<ActionResult> Edit(InfoViewModel model)
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
                userinDb.Address = user.Address;
                userinDb.Specialty = user.Specialty;
                IdentityResult result = await UserManager.UpdateAsync(userinDb);
               

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    AddErrors(result);
            // If we got this far, something failed, redisplay form
            } 
        return View(model);
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