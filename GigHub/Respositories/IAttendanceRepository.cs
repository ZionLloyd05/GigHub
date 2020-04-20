using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Respositories
{
    public interface IAttendanceRepository
    {
        Attendance GetAttendancesForGig(int id, string userId);
        IEnumerable<Attendance> GetFutureAttendances(string userId);
        void Add(Attendance attendance);
        void Remove(Attendance attendanceInDb);
    }
}