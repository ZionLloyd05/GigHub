using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Respositories
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetGenres();
    }
}