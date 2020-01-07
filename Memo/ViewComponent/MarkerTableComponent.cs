using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Memo.Data;
using Memo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Memo.ViewComponent
{
    public class MarkerTableComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MarkerTableComponent(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(Request.HttpContext.User);


            var memoMarkers = _context
                .MemoMarkers
                .Where(marker => !marker.Obsolete)
                .Where(marker => marker.User == user);

            return View("MemoTable", memoMarkers);
        }
    }
}