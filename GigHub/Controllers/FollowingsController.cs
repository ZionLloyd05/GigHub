using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }

        
        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();

            if(dto == null)
                return BadRequest();

            if (_context.Followings.Any(f => f.FollowerId == userId && f.FolloweeId == dto.FolloweeId))
                return BadRequest("Following already exists");

            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = dto.FolloweeId
            };

            _context.Followings.Add(following);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Disfollow(string id)
        {
            var userId = User.Identity.GetUserId();

            if (id == "")
                return BadRequest();

            var follow = _context.Followings
                    .Where(f => f.FolloweeId == id && f.FollowerId == userId)
                    .SingleOrDefault();

            _context.Followings.Remove(follow);
            _context.SaveChanges();

            return Ok();
        }

        
    }
}