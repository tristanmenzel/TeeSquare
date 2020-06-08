using Microsoft.AspNetCore.Mvc;

namespace TeeSquare.DemoApi.Controllers
{
    public class DefaultRouteController : Controller
    {
        // GET
        public int Index()
        {
            return 0;
        }

        public int GetNum(int id)
        {
            return id;
        }
    }
}
