using GigHub.Models;

namespace GigHub.Respositories
{
    public interface IFollowingRepository
    {
        Following GetUserFollow(string id, string userId);
        void Add(Following following);
        void Remove(Following following);
    }
}