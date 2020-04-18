using GigHub.Models;

namespace GigHub.ViewModels
{
    public class GigsDetailViewModel
    {
        public Gig Gig { get; set; }
        public bool IsAttendingGig { get; set; }
        public bool IsFollowingGigArtist { get; set; }
    }
}