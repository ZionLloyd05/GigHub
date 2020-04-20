using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Respositories
{
    public interface IGigRepository
    {
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        IEnumerable<Gig> GetUpcomingGigs();
        Gig GetUserGigithArtistAndGenre(int gigId);
        void Add(Gig gig);
    }
}