using GigHub.Models;
using System.Linq;

namespace GigHub.Respositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Following following)
        {
            _context.Followings.Add(following);
        }

        public Following GetUserFollow(string id, string userId)
        {
            return _context.Followings
                    .Where(f => f.FolloweeId == id && f.FollowerId == userId)
                    .SingleOrDefault();
        }

        public void Remove(Following following)
        { 
            _context.Followings.Remove(following);
        }

    }
}