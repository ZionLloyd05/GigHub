﻿using GigHub.Dtos;
using GigHub.Models;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Web.Http;

namespace GigHub.Controllers
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public AttendancesController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
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
            Attendance attendanceInDb = _unitOfWork.Attendances.GetAttendancesForGig(id, userId);
                            
            if (attendanceInDb == null)
                return NotFound();

            if (attendanceInDb.AttendeeId != userId)
                return BadRequest("Unauthorized");

            _context.Attendances.Remove(attendanceInDb);
            _context.SaveChanges();

            return Ok();
        }

    }
}