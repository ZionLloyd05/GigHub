using GigHub.Models;
using System.Linq;

namespace GigHub.Respositories
{
    public class FollowingRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public Following GetUserFollow(string id, string userId)
        {
            return _context.Followings
                    .Where(f => f.FolloweeId == id && f.FollowerId == userId)
                    .SingleOrDefault();
        }
    }
}