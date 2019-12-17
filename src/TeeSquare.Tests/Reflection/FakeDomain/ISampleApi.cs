namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public interface ISampleApi
    {
        Book GetBook(int id);

        void SaveBook(Book book);
    }
}
