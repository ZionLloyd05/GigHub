using GigHub.Models;
using GigHub.Respositories;

namespace GigHub.Persistence
{
    public class UnitOfWork
    {
        public GigRepository Gigs { get; private set; }
        public AttendanceRepository Attendances { get; private set; }
        public GenreRepository Genres { get; private set; }
        public FollowingRepository Follows { get; private set; }

        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Gigs = new GigRepository(_context);
            Attendances = new AttendanceRepository(_context);
            Genres = new GenreRepository(_context);
            Follows = new FollowingRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}