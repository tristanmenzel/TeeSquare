using System.Collections.Generic;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Movie
    {
        public string Name { get;set; }
        public int Year { get; set;}
        public string Director { get; set;}
        public Genre Genre { get; set;}
    }

    public enum Genre
    {
        Drama,
        Doco
    }

    public class Indexes
    {
        public IDictionary<int, Movie[]> MoviesByYear { get; set; }
        public IDictionary<string, Movie[]> MoviesByDirector { get; set;}
        public IDictionary<Genre, Movie[]> MoviesByGenre { get; set;}
    }
}
