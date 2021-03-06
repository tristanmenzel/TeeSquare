namespace RPS.myProjects.Infrastructure.Util
{
    public abstract class Result
    {
        public static SuccessResult<T> Success<T>(T val)
        {
            return new(val);
        }

        public static FailResult<Unbound> Fail(string message)
        {
            return new(message);
        }

        public class Unbound
        {

        }
    }
}
