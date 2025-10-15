using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinhocOnline.Models;

namespace TinhocOnline.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class TeachersController : Controller
    {
         private readonly DataContext _context;

        public TeachersController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lọc chỉ những người có Role = 'Teacher'
            var teachers = _context.tblUsers
                .Where(u => u.Role == "Teacher")
                .ToList();

            return View(teachers);
        }
    }
}