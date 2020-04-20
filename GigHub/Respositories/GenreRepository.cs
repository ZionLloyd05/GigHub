using GigHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Respositories
{
    public class GenreRepository : IGenreRepository
    {

        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }

    }
}