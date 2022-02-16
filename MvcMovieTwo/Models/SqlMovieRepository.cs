using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovieTwo.Data;

namespace MvcMovieTwo.Models
{
    public class SqlMovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlMovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> Add(Movie movie, IdentityUser user)
        {
            movie.UserId = user.Id;
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public Movie Delete(Guid id)
        {
            Movie movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            return movie;
        }


        public IEnumerable<Movie> GetAllMovie()
        {
            return _context.Movies;
        }

        public Movie GetMovie(Guid id)
        {
            return _context.Movies.Find(id);
        }

        public async Task<Movie[]> GetUserMoviesAsync(IdentityUser user)
        {
            return await _context.Movies
                .Where(x => x.UserId == user.Id)
                .ToArrayAsync();
              
        }

        public Movie Update(Movie movieChanges)
        {
            _context.Movies.Update(movieChanges);
            _context.SaveChanges();
            return movieChanges;
        }
    }
}
