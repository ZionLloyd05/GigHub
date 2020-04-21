
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();

            if(dto == null || string.IsNullOrEmpty(dto.FolloweeId))
                return BadRequest();

            if (!_unitOfWork.Follows.IsArtistToFollowExist(dto.FolloweeId))
                return NotFound();

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