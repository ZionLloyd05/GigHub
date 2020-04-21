using System;
using System.Collections.Generic;

namespace GigHub.Core.Models
{
    public class Gig
    {
        public int Id { get; set; }

        public ApplicationUser Artist { get; set; }

        public string ArtistId { get; set; }

        public DateTime DateTime { get; set; }

        public string Venue { get; set; }

        public Genre Genre { get; set; }

        public byte GenreId { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
    }
}