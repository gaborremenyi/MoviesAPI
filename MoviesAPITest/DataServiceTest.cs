using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesAPI.Data.Test
{
    [TestClass]
    public class DataServiceTest
    {
        private IDataService dataService;

        public DataServiceTest()
        {
            InMemoryDataService.movies = new List<Movie>() {
                new Movie(){MovieId = 1, Title="Test Movie 1", YearOfRelease = 2000, Genres = new List<Genre>(){ Genre.Drama } },
                new Movie(){MovieId = 2, Title="Test Movie 2", YearOfRelease = 2000, Genres = new List<Genre>(){ Genre.Drama } },
                new Movie(){MovieId = 3, Title="Test Movie 3", YearOfRelease = 2000, Genres = new List<Genre>(){ Genre.Drama, Genre.Fiction } },
                new Movie(){MovieId = 4, Title="Test Movie 4", YearOfRelease = 2010, Genres = new List<Genre>(){ Genre.Action, Genre.Fiction } },
                new Movie(){MovieId = 5, Title="Test Movie 5", YearOfRelease = 2010, Genres = new List<Genre>(){ Genre.Fiction } },
                new Movie(){MovieId = 6, Title="Test Movie 6", YearOfRelease = 2015, Genres = new List<Genre>(){ Genre.Horror } }
            };

            InMemoryDataService.users = new List<User>() {
                new User(){ UserId = 1, UserName = "TestUser1" },
                new User(){ UserId = 2, UserName = "TestUser2" }
            };

            InMemoryDataService.ratings = new List<Rating>() {
                new Rating() { RatingId = 1, UserId = 1, MovieId = 1, Value = 4 },
                new Rating() { RatingId = 2, UserId = 1, MovieId = 2, Value = 5 },
                new Rating() { RatingId = 3, UserId = 1, MovieId = 3, Value = 3 },
                new Rating() { RatingId = 4, UserId = 1, MovieId = 4, Value = 2 },
                new Rating() { RatingId = 5, UserId = 1, MovieId = 5, Value = 1 },
                new Rating() { RatingId = 6, UserId = 2, MovieId = 1, Value = 4 },
                new Rating() { RatingId = 7, UserId = 2, MovieId = 2, Value = 4 },
                new Rating() { RatingId = 8, UserId = 2, MovieId = 3, Value = 4 },
                new Rating() { RatingId = 9, UserId = 2, MovieId = 4, Value = 1 },
                new Rating() { RatingId = 10, UserId = 2, MovieId = 5, Value = 2 }
        };

            dataService = new InMemoryDataService();
        }

        [TestMethod]
        public void GetMoviesTest()
        {
            var titleFilter = "Movie";
            var yearFilter = 2000;
            var genresFilter = new List<Genre>() { Genre.Drama, Genre.Fiction };

            Assert.AreEqual(6, dataService.GetMovies(titleFilter).Count);
            Assert.AreEqual(3, dataService.GetMovies(null, yearFilter).Count);
            Assert.AreEqual(5, dataService.GetMovies(null, null, genresFilter).Count);

            titleFilter = "movie 4";
            yearFilter = 2010;
            genresFilter = new List<Genre>() { Genre.Action };

            Assert.AreEqual(1, dataService.GetMovies(null, yearFilter, genresFilter).Count);
            Assert.AreEqual(1, dataService.GetMovies(titleFilter, yearFilter).Count);
            Assert.AreEqual(1, dataService.GetMovies(null, yearFilter, genresFilter).Count);

            titleFilter = "movie";
            yearFilter = 2000;
            genresFilter = new List<Genre>() { Genre.Fiction };

            Assert.AreEqual(1, dataService.GetMovies(titleFilter, yearFilter, genresFilter).Count);
        }

        [TestMethod]
        public void GetTop5RatedMoviesTest()
        {
            var topRateMovies = dataService.GetTop5RatedMovies();

            Assert.IsNotNull(topRateMovies);
            Assert.AreEqual(5, topRateMovies.Count);
            Assert.AreEqual(2, topRateMovies.FirstOrDefault().MovieId);
            Assert.AreEqual(4.5, topRateMovies.FirstOrDefault().AverageRating);

            foreach (var movie in topRateMovies)
            {
                Assert.IsTrue(movie.AverageRating % 0.5 == 0);
            }
        }

        [TestMethod]
        public void GetTop5RatedMoviesByUserTest()
        {
            var userId = 1;
            var topRateMovies = dataService.GetTop5RatedMovies(userId);

            Assert.IsNotNull(topRateMovies);
            Assert.AreEqual(5, topRateMovies.Count);
            Assert.AreEqual(2, topRateMovies.FirstOrDefault().MovieId);
            Assert.AreEqual(5, topRateMovies.FirstOrDefault().AverageRating);
        }

        [TestMethod]
        public void AddRatingTest()
        {
            var rating1 = new Rating() { RatingId = 1, UserId = 1, MovieId = 6, Value = 3 };
            var rating2 = new Rating() { RatingId = 2, UserId = 1, MovieId = 6, Value = 4 };

            dataService.AddRating(rating1);

            Assert.IsNotNull(dataService.GetRating(1, 6));
            Assert.AreEqual(rating1.Value, dataService.GetRating(1,6).Value);

            dataService.AddRating(rating2);

            Assert.IsNotNull(dataService.GetRating(1, 6));
            Assert.AreEqual(rating2.Value, dataService.GetRating(1, 6).Value);
        }
    }
}
