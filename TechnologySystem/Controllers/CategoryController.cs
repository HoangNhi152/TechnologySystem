using Microsoft.Ajax.Utilities;
using System.Linq;
using System.Web.Mvc;
using TechnologySystem.Models;

namespace TechnologySystem.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext _context;
        public CategoryController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Categories
        public ActionResult Index(string searchString)
        {
            var categories = _context.Categories.ToList();

            if (!searchString.IsNullOrWhiteSpace())
            {
                categories = categories.Where(c => c.CategoryName.Contains(searchString)).ToList();
            }

            return View(categories);
        }


    }
}