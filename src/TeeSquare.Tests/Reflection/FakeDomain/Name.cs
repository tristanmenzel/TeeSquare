namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Name
    {
        public string FirstName { get; set; }
        public Title Title { get; set; }
        public string LastName { get; set; }
        
        public static string IgnoreMe { get; set; }
    }
}