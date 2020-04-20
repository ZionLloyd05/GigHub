using System.Linq;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Respositories
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
                .SingleOrDefault(f => f.FolloweeId == id && f.FollowerId == userId);
        }

        public void Remove(Following following)
        { 
            _context.Followings.Remove(following);
        }

    }
}