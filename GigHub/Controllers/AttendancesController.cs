using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var userId = User.Identity.GetUserId();
            if (_context.Attendances.Any(a => a.GigId == dto.GigId && a.AttendeeId == userId))
                return BadRequest("The attendance already exist");

            var attendance = new Attendance
            {
                GigId = dto.GigId,
                AttendeeId = User.Identity.GetUserId()
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DisAttend(int id)
        {
            if (id < 1)
                return BadRequest();

            var userId = User.Identity.GetUserId();

            var attendanceInDb = _context.Attendances
                .Where(a => a.GigId == id && a.AttendeeId == userId)
                .SingleOrDefault();

            if (attendanceInDb == null)
                return NotFound();

            _context.Attendances.Remove(attendanceInDb);
            _context.SaveChanges();

            return Ok();
        }
    }
}