using System.Collections.Generic;
using System.Globalization;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Library
    {
        public string Name { get; set; }
        
        public Location Location { get; set; }

        public int SquareMeters { get; set; }

        public int? Levels { get; set; }

        public List<Book> AllBooks { get; set; }

        public Book[] TopBorrowed { get; set; }
    }
}