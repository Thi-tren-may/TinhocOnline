using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinhocOnline.Models;
using TinhocOnline.Areas.Admin.Models;

namespace TinhocOnline.Areas.Admin.Components
{
    [ViewComponent(Name = "AdminMenu")]
    public class AdminMenuComponent : ViewComponent
    {
        private readonly DataContext _context;
        public AdminMenuComponent(DataContext context)
        {
            _context = context;
        }
        // ---- Hàm chính sẽ chạy khi gọi @await Component.InvokeAsync("AdminMenu") ----
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy tất cả menu đang hoạt động
            var mnList = (from mn in _context.AdminMenus
                        where(mn.IsActive == true)
                        select mn).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", mnList));
        }        
    }
}