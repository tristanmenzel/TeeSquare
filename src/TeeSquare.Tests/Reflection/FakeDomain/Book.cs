using System;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Book
    {
        public string Title { get; set; }
        public Name Author { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime FirstPublished { get; set; }
        public DateTimeOffset? LastRevisedOn { get; set; }
        public bool? ReviewedPositively { get; set; }
        public Audience? RecommendedAudience { get; set; }
    }
}