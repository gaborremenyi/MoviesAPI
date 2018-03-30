using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace MoviesAPI.Models
{
    public enum Genre { Action, Comedy, Drama, Fiction, Romance, Horror };

    public class Movie
    {
        public int MovieId { get; set; }

        public string Title { get; set; }

        public int YearOfRelease { get; set; }

        public TimeSpan RunningTime { get; set; }

        public double AverageRating { get; set; }

        [JsonIgnore]
        public List<Genre> Genres { get; set; }
    }
}
