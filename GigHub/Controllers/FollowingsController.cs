using GigHub.Dtos;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using GigHub.Core.Models;

namespace GigHub.Controllers
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly UnitOfWork _unitOfWork;

        public FollowingsController()
        {;
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
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

            _unitOfWork.Follows.Add(following);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Disfollow(string id)
        {
            var userId = User.Identity.GetUserId();

            if (id == "")
                return BadRequest();
            Following follow = _unitOfWork.Follows.GetUserFollow(id, userId);

            _unitOfWork.Follows.Remove(follow);
            _unitOfWork.Complete();

            return Ok();
        }


    }
}