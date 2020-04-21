using GigHub.Core.Models;
using GigHub.Core.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;

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

        public bool IsArtistToFollowExist(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(id);
            return user != null;
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