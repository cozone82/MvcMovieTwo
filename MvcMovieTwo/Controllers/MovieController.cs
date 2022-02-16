using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcMovieTwo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MvcMovieTwo.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _repository;
        private readonly UserManager<IdentityUser> _userManager;

        public MovieController(IMovieRepository repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Challenge();
            }


            var movies = await _repository.GetUserMoviesAsync(currentUser);

            var model = new MovieViewModel()
            { 
                Movies = movies
            };

            return View(model);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
           

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Challenge();
            }

            if (ModelState.IsValid)
            {
                var newMovie = await _repository.Add(movie, currentUser);
                return RedirectToAction("index", new { id = newMovie.Id });
            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(Guid id)
        {
            Movie movie = _repository.GetMovie(id);
            Movie updateMovie = new Movie
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Genre = movie.Genre,
                Price = movie.Price

            };
            return View(updateMovie);
        }

        [HttpPost]
        public IActionResult Edit(Movie model)
        {
            if (ModelState.IsValid)
            {
                Movie movie = _repository.GetMovie(model.Id);
                movie.Title = model.Title;
                movie.ReleaseDate = model.ReleaseDate;
                movie.Genre = model.Genre;
                movie.Price = model.Price;

                _repository.Update(movie);
                return RedirectToAction("index");

            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(Guid Id)
        {
            _repository.Delete(Id);
            return RedirectToAction("Index");

        }
    }
}
