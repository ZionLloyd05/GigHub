using GigHub.Dtos;
using GigHub.Models;
using GigHub.Respositories;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers
{
    [Authorize]
    public class FollowingsController : ApiController
    {

        private readonly ApplicationDbContext _context;
        private readonly FollowingRepository _followingRepository;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
            _followingRepository = new FollowingRepository(_context);
        }


        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();

            if(dto == null)
                return BadRequest();

            if (_followingRepository.GetUserFollow(dto.FolloweeId, userId) != null)
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
            Following follow = _followingRepository.GetUserFollow(id, userId);

            _context.Followings.Remove(follow);
            _context.SaveChanges();

            return Ok();
        }


    }
}