using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Data;
using MoviesAPI.Exceptions;
using MoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MoviesAPI.Controllers
{
    [Route("api")]
    public class MoviesController : BaseController
    {
        public MoviesController(IDataService dataService) : base(dataService) { }

        // GET api/A
        [HttpGet("A")]
        public IEnumerable<Movie> GetMovies(string title, int? year, List<string> genres)
        {
            try
            {
                var parsedGenres = genres
                    .Select(s => { Genre g; return Enum.TryParse(s, out g) ? g : (Genre?)null; })
                    .Where(i => i.HasValue)
                    .Select(i => i.Value)
                    .ToList();

                // 400 (if invalid / no criteria is given)
                if (string.IsNullOrEmpty(title) && year == null && (genres == null || !genres.Any()))
                    throw new HttpException(HttpStatusCode.BadRequest);

                var movies = dataService.GetMovies(title, year, parsedGenres);

                // 404 (if no movie is found based on the criteria)
                if (movies == null || !movies.Any())
                    throw new HttpException(HttpStatusCode.NotFound);

                return movies;
            }
            catch (HttpException) { throw; }
            catch (Exception) { throw new HttpException(HttpStatusCode.InternalServerError); }
        }

        // GET api/B
        [HttpGet("B")]
        public IEnumerable<Movie> GetTop5RatedMovies()
        {
            try
            {
                var movies = dataService.GetTop5RatedMovies();

                // 404 (if no movie is found based on the criteria)
                if (movies == null || !movies.Any())
                    throw new HttpException(HttpStatusCode.NotFound);

                return movies;
            }
            catch (HttpException) { throw; }
            catch (Exception) { throw new HttpException(HttpStatusCode.InternalServerError); }
        }

        // GET api/C
        [HttpGet("C")]
        public IEnumerable<Movie> GetTop5RatedMovies(int? userId)
        {
            try
            {
                // 400 (if invalid / no criteria is given)
                if (userId == null)
                    throw new HttpException(HttpStatusCode.BadRequest);

                var movies = dataService.GetTop5RatedMovies(userId);

                // 404 (if no movie is found based on the criteria)
                if (movies == null || !movies.Any())
                    throw new HttpException(HttpStatusCode.NotFound);

                return movies;
            }
            catch (HttpException) { throw; }
            catch (Exception) { throw new HttpException(HttpStatusCode.InternalServerError); }
        }

        // POST api/D
        [HttpPost("D")]
        public void AddOrUpdateUserRating([FromBody]Rating rating)
        {
            try
            {
                // 400 (if rating is an invalid value)
                if (rating == null || rating.Value > 5 || rating.Value < 1)
                    throw new HttpException(HttpStatusCode.BadRequest);

                // 404 (if no movie is found based on the criteria)
                if (dataService.GetMovie(rating.MovieId) == null ||
                    dataService.GetUser(rating.UserId) == null)
                    throw new HttpException(HttpStatusCode.NotFound);

                dataService.AddRating(rating);
            }
            catch (HttpException) { throw; }
            catch (Exception) { throw new HttpException(HttpStatusCode.InternalServerError); }
        }
    }
}
