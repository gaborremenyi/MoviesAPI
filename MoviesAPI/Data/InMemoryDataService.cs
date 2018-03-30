using System;
using System.Collections.Generic;
using System.Linq;
using MoviesAPI.Models;

namespace MoviesAPI.Data
{
    public class InMemoryDataService : IDataService
    {
        #region STATIC DATA
        public static List<Movie> movies = new List<Movie>() {
            new Movie(){MovieId = 1, Title="The Shawshank Redemption", YearOfRelease = 1994, RunningTime = new TimeSpan(2,30,20), AverageRating = 0, Genres = new List<Genre>(){ Genre.Drama } },
            new Movie(){MovieId = 2, Title="The Godfather", YearOfRelease = 1972, RunningTime = new TimeSpan(2,30,20), AverageRating = 0, Genres = new List<Genre>(){ Genre.Drama } },
            new Movie(){MovieId = 3, Title="The Godfather: Part II", YearOfRelease = 1974, RunningTime = new TimeSpan(2,30,20), AverageRating = 0, Genres = new List<Genre>(){ Genre.Drama } },
            new Movie(){MovieId = 4, Title="The Dark Knight", YearOfRelease = 2008, RunningTime = new TimeSpan(2,30,20), AverageRating = 0, Genres = new List<Genre>(){ Genre.Action, Genre.Fiction } },
            new Movie(){MovieId = 5, Title="12 Angry Men", YearOfRelease = 1957, RunningTime = new TimeSpan(2,30,20), AverageRating = 0, Genres = new List<Genre>(){ Genre.Drama } },
            new Movie(){MovieId = 6, Title="Schindler's List", YearOfRelease = 1993, RunningTime = new TimeSpan(2,30,20), AverageRating = 0, Genres = new List<Genre>(){ Genre.Drama } },
            new Movie(){MovieId = 7, Title="The Lord of the Rings: The Return of the King", YearOfRelease = 2003, RunningTime = new TimeSpan(2,30,20), AverageRating = 0, Genres = new List<Genre>(){ Genre.Fiction } },
            new Movie(){MovieId = 8, Title="Pulp Fiction", YearOfRelease = 1994, RunningTime = new TimeSpan(2,30,20), AverageRating = 0, Genres = new List<Genre>(){ Genre.Action, Genre.Comedy } },
            new Movie(){MovieId = 9, Title="The Good, the Bad and the Ugly", YearOfRelease = 1966, RunningTime = new TimeSpan(2,30,20), AverageRating = 0, Genres = new List<Genre>(){ Genre.Comedy } },
            new Movie(){MovieId = 10, Title="Fight Club", YearOfRelease = 1999, RunningTime = new TimeSpan(2,30,20), AverageRating = 0, Genres = new List<Genre>(){ Genre.Action, Genre.Drama } }
        };

        public static List<User> users = new List<User>() {
            new User(){ UserId = 1, UserName = "User1" },
            new User(){ UserId = 2, UserName = "User2" },
            new User(){ UserId = 3, UserName = "User3" },
            new User(){ UserId = 4, UserName = "User4" },
            new User(){ UserId = 5, UserName = "User5" },
            new User(){ UserId = 6, UserName = "User6" },
            new User(){ UserId = 7, UserName = "User7" },
            new User(){ UserId = 7, UserName = "User8" }
        };

        public static List<Rating> ratings = new List<Rating>();

        private static object _lock = new object();

        static InMemoryDataService()
        {
            int id = 1;
            Random rnd = new Random();

            foreach (var user in users)
            {
                foreach (var movie in movies)
                {
                    if (rnd.Next(0, 2) > 0)
                        ratings.Add(new Rating() { RatingId = id++, UserId = user.UserId, MovieId = movie.MovieId, Value = rnd.Next(1, 6) });
                }
            }
        }
        #endregion

        #region Movie
        public Movie GetMovie(int movieId)
        {
            return movies.Where(x => x.MovieId == movieId).FirstOrDefault();
        }

        public ICollection<Movie> GetMovies(string title = null, int? yearOfRelease = null, List<Genre> genres = null)
        {
            return (from movie in movies
                   join rating in ratings on movie.MovieId equals rating.MovieId into RatingLeftJoin
                   from r in RatingLeftJoin.DefaultIfEmpty()
                   where (title == null || movie.Title.ToLower().Contains(title.ToLower())) &&
                   (yearOfRelease == null || movie.YearOfRelease == yearOfRelease) &&
                   (genres == null || !genres.Any() || movie.Genres.Where(g => genres.Contains(g)).Any())
                   group r by new { movie.MovieId, movie.Title, movie.YearOfRelease, movie.RunningTime, movie.Genres } into MovieGroup
                   select new Movie
                   {
                       MovieId = MovieGroup.Key.MovieId,
                       Title = MovieGroup.Key.Title,
                       YearOfRelease = MovieGroup.Key.YearOfRelease,
                       RunningTime = MovieGroup.Key.RunningTime,
                       Genres = MovieGroup.Key.Genres,
                       AverageRating = Math.Round(2 * MovieGroup.Average(x => x != null ? x.Value : 0)) / 2
                   }).ToList();
        }

        public ICollection<Movie> GetTop5RatedMovies(int? userId = null)
        {
            return (from movie in movies
                    join rating in ratings on movie.MovieId equals rating.MovieId
                    where (userId == null || rating.UserId == userId)
                    group rating by new { movie.MovieId, movie.Title, movie.YearOfRelease, movie.RunningTime, movie.Genres } into MovieGroup
                    orderby MovieGroup.Average(x => x.Value) descending, MovieGroup.Key.Title
                    select new Movie
                    {
                        MovieId = MovieGroup.Key.MovieId,
                        Title = MovieGroup.Key.Title,
                        YearOfRelease = MovieGroup.Key.YearOfRelease,
                        RunningTime = MovieGroup.Key.RunningTime,
                        Genres = MovieGroup.Key.Genres,
                        AverageRating = Math.Round(2 * MovieGroup.Average(x => x.Value)) / 2
                    }).Take(5).ToList();
        }
        #endregion

        #region User
        public User GetUser(int userId)
        {
            return users.Where(x => x.UserId == userId).FirstOrDefault();
        }
        #endregion

        #region Rating
        public Rating GetRating(int userId, int movieId)
        {
            return ratings.Where(x => x.UserId == userId && x.MovieId == movieId).FirstOrDefault();
        }

        public void AddRating(Rating rating)
        {
            lock (_lock)
            {
                var existingRating = GetRating(rating.UserId, rating.MovieId);
                if (existingRating == null)
                {
                    var ratingId = ratings.Max(x => x.RatingId) + 1;
                    ratings.Add(new Rating() { RatingId = ratingId, UserId = rating.UserId, MovieId = rating.MovieId, Value = rating.Value });
                }
                else
                {
                    existingRating.Update(rating);
                }
            }
        }
        #endregion
    }
}
