using System;
using System.Collections.Generic;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Member
    {
        public Guid Id { get; set; }
        
        public IEnumerable<Book> PreviouslyBorrowedBooks { get; set; }
        
        public IDictionary<string, Book> CurrentBooks { get; set; }
        
        public KeyValuePair<string, int>[] Ratings { get; set; }
    }
}