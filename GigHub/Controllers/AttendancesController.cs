using GigHub.Core.Dtos;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using GigHub.Core.Models;

namespace GigHub.Controllers
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly UnitOfWork _unitOfWork;

        public AttendancesController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }

        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();

            var attendanceForGig = _unitOfWork.Attendances.GetAttendancesForGig(dto.GigId, userId);

            if (attendanceForGig != null)
                return BadRequest("The attendance already exist");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = User.Identity.GetUserId()
            };

            _unitOfWork.Attendances.Add(attendance);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DisAttend(int id)
        {
            if (id < 1)
                return BadRequest();

            var userId = User.Identity.GetUserId();
            Attendance attendanceInDb = _unitOfWork.Attendances.GetAttendancesForGig(id, userId);
                            
            if (attendanceInDb == null)
                return NotFound();

            if (attendanceInDb.AttendeeId != userId)
                return BadRequest("Unauthorized");

            _unitOfWork.Attendances.Remove(attendanceInDb);

            _unitOfWork.Complete();

            return Ok();
        }

    }
}