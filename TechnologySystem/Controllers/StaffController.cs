using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TechnologySystem.Models;
using TechnologySystem.Utils;
using TechnologySystem.ViewModels;

namespace TechnologySystem.Controllers
{
    public class StaffController : Controller
    {
        // GET: Admin/Account
        public async Task<ActionResult> Index()
        {
            var traineeRole = await _context.Roles.SingleOrDefaultAsync(r => r.Name == Role.Trainee);
            var model = new UsersGroupViewModel()
            {
                Trainees = await _context.Users
                    .Where(u => u.Roles.Any(r => r.RoleId == traineeRole.Id))
                    .ToListAsync(),
            };
            return View(model);
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
    }
}