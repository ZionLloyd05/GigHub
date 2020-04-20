using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Respositories
{
    public class GigRepository
    {
        private readonly ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            if (userId == "")
                return null;

            return _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();
        }

        public Gig GetUserGigithArtistAndGenre(int gigId)
        {
            if (gigId < 1)
                return null;

            return _context.Gigs
              .Where(g => g.Id == gigId)
              .Include(g => g.Artist)
              .Include(g => g.Genre)
              .FirstOrDefault();
        }


        public IEnumerable<Gig> GetUpcomingGigs()
        {
            return _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now).ToList();
        }

        internal void Add(Gig gig)
        {
            _context.Gigs.Add(gig);
        }
    }
}