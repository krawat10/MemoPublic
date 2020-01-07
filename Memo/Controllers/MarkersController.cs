using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Memo.Data;
using Memo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Memo.Controllers
{
    [Authorize]
    public class MarkersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private IQueryable<MemoMarker> MemoMarkers => 
            _dbContext.MemoMarkers.Where(marker => !marker.Obsolete);

        public MarkersController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Details(long id)
        {
            var memoMarker = MemoMarkers.FirstOrDefault(marker => marker.Id == id);

            return PartialView(memoMarker);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MemoMarkerRequest request)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            _dbContext.Add(
                new MemoMarker(request.Name, request.Description, request.Coordinates, user));
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Map", "Home");
        }
        
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MemoMarkerRequest request)
        {
            var memoMarker = MemoMarkers.FirstOrDefault(marker => marker.Id == id);

            memoMarker.UpdateName(request.Name);
            memoMarker.UpdateDescription(request.Description);
            memoMarker.UpdateCoordinates(request.Coordinates);

            _dbContext.Update(memoMarker);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Map", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var memoMarker = MemoMarkers.FirstOrDefault(marker => marker.Id == id);

            if (memoMarker == null)
            {
                return NotFound();
            }

            memoMarker.DeleteMarker();
            _dbContext.Update(memoMarker);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Map", "Home");
        }

        public async Task<IActionResult> SetPublic(long id)
        {
            var memoMarker = MemoMarkers
                .FirstOrDefault(marker => marker.Id == id);

            if (memoMarker == null)
            {
                return new NotFoundResult();
            }

            memoMarker.SetAsShared();

            _dbContext.Update(memoMarker);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Map", "Home");
        }


        public async Task<IActionResult> SetAsCenter(long id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var memoMarkers = MemoMarkers
                .Where(marker => marker.User == user);

            foreach (var memoMarker in memoMarkers)
            {
                if (memoMarker.Id == id)
                {
                    memoMarker.SetAsCenterPoint();
                }
                else
                {
                    memoMarker.SetAsNotCenterPoint();
                }

                _dbContext.Update(memoMarker);
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Map", "Home");
        }

        public async Task<IActionResult> SetPrivate(long id)
        {
            var memoMarker = MemoMarkers
                .FirstOrDefault(marker => marker.Id == id);

            if (memoMarker == null)
            {
                return new NotFoundResult();
            }

            memoMarker.SetAsNotShared();

            _dbContext.Update(memoMarker);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Map", "Home");
        }
    }
}