namespace MoviesAPI.Models
{
    public class Rating
    {
        public int RatingId { get; set; }

        public int UserId { get; set; }

        public int MovieId { get; set; }

        public int Value { get; set; }

        public void Update(Rating rating)
        {
            this.Value = rating.Value;
        }

        public override bool Equals(object other)
        {
            var compareWith = other as Rating;
            if (compareWith == null)
                return false;
            return this.UserId == compareWith.UserId && this.MovieId == compareWith.MovieId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
