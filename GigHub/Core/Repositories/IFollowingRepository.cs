using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IFollowingRepository
    {
        Following GetUserFollow(string id, string userId);
        void Add(Following following);
        void Remove(Following following);
    }
}