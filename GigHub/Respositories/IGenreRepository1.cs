using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Respositories
{
    public interface IGenreRepository1
    {
        IEnumerable<Genre> GetGenres();
    }
}