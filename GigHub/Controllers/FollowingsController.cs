using GigHub.Dtos;
using GigHub.Models;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers
{
    [Authorize]
    public class FollowingsController : ApiController
    {

        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }


        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();

            if(dto == null)
                return BadRequest();

            if (_unitOfWork.Follows.GetUserFollow(dto.FolloweeId, userId) != null)
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
            Following follow = _unitOfWork.Follows.GetUserFollow(id, userId);

            _context.Followings.Remove(follow);
            _context.SaveChanges();

            return Ok();
        }


    }
}