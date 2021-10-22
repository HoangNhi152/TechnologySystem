using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnologySystem.Models;

namespace TechnologySystem.Controllers
{
	public class TrainerController : Controller
	{
		// GET: Trainer
		private ApplicationDbContext _context;
		public TrainerController()
		{
			_context = new ApplicationDbContext();
		}
		// GET: TrainerInfo
		public ActionResult Index()
		{
			var userId = User.Identity.GetUserId();
			var trainer = _context.Users.SingleOrDefault(u => u.Id.Equals(userId));

			if (trainer == null) return HttpNotFound();
			return View(trainer);
		}
	}
}