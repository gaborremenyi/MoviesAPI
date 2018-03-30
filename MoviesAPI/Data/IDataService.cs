using MoviesAPI.Models;
using System.Collections.Generic;

namespace MoviesAPI.Data
{
    public interface IDataService
    {
        Movie GetMovie(int movieId);

        User GetUser(int userId);

        Rating GetRating(int userId, int movieId);

        ICollection<Movie> GetMovies(string title = null, int? yearOfRelease = null, List<Genre> genres = null);

        ICollection<Movie> GetTop5RatedMovies(int? userId = null);

        void AddRating(Rating rating);
    }
}
