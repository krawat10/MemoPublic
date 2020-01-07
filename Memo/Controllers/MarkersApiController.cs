using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Memo.Data;
using Memo.DTOs;
using Memo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Memo.Controllers
{
    [Route("api/Markers")]
    [Authorize]
    public class MarkersApiController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        private IQueryable<MemoMarker> MemoMarkers =>
            _dbContext.MemoMarkers.Where(marker => !marker.Obsolete);

        public MarkersApiController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<MemoMarkerDto>> Get()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return MemoMarkers
                .Where(marker => marker.User == user)
                .Select(marker => new MemoMarkerDto
                {
                    Id = marker.Id,
                    Coordinates = marker.Coordinates,
                    Created = marker.Created,
                    Name = marker.Name,
                    UserName = marker.User.UserName,
                    Description = marker.Description,
                    Updated = marker.Updated,
                    IsCenterPoint = marker.IsCenterPoint
                });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/api/Markers/Shared")]
        public async Task<IEnumerable<MemoMarkerDto>> GetShared()
        {
            return MemoMarkers
                .Where(marker => marker.Shared)
                .Select(marker => new MemoMarkerDto
                {
                    Id = marker.Id,
                    Coordinates = marker.Coordinates,
                    Created = marker.Created,
                    Name = marker.Name,
                    UserName = marker.User.UserName,
                    Description = marker.Description,
                    Updated = marker.Updated,
                    IsCenterPoint = marker.IsCenterPoint
                });
        }

        [HttpGet("{id}")]
        public async Task<MemoMarkerDto> Get(long id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return MemoMarkers
                .Where(marker => marker.User == user)
                .Select(marker => new MemoMarkerDto
                {
                    Id = marker.Id,
                    Coordinates = marker.Coordinates,
                    Created = marker.Created,
                    Name = marker.Name,
                    UserName = marker.User.UserName,
                    Description = marker.Description,
                    Updated = marker.Updated,
                    IsCenterPoint = marker.IsCenterPoint
                })
                .FirstOrDefault(marker => marker.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MemoMarkerRequest request)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            _dbContext.Add(new MemoMarker(request.Name, request.Description, request.Coordinates, user));

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Map", "Home");
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] MemoMarkerRequest request)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var memoMarker = MemoMarkers
                .FirstOrDefault(marker => marker.Id == id && marker.User == user);

            memoMarker.UpdateName(request.Name);
            memoMarker.UpdateDescription(request.Description);
            memoMarker.UpdateCoordinates(request.Coordinates);

            _dbContext.Update(memoMarker);
            await _dbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var memoMarker = MemoMarkers
                .FirstOrDefault(marker => marker.Id == id && marker.User == user);

            memoMarker.DeleteMarker();
            _dbContext.Update(memoMarker);
            await _dbContext.SaveChangesAsync();
        }
    }
}