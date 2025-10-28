using Microsoft.AspNetCore.Mvc;
using TinhocOnline.Models;

namespace TinhocOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly DataContext _context;

        // GET: DashboardController
        public ActionResult Index()
        {
            return View();
        }
    }
}
