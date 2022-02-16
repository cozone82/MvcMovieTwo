using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovieTwo.Models
{
    public interface IMovieRepository
    {
        Task<Movie[]> GetUserMoviesAsync(IdentityUser user);
        Movie GetMovie(Guid id);
        IEnumerable<Movie> GetAllMovie();
        Task <Movie> Add(Movie movie, IdentityUser user);
        Movie Update(Movie movieChanges);
        Movie Delete(Guid id);
    }
}
