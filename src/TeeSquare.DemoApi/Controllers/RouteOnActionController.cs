using Microsoft.AspNetCore.Mvc;

namespace TeeSquare.DemoApi.Controllers
{
    [Route("api")]
    public class RouteOnActionController : Controller
    {
        [Route("route-number-one")]
        public string RouteNumberOne()
        {
            return "RouteNumberOne";
        }

        [Route("/alt-api/route-number-two/{id}")]
        [HttpGet]
        public string RouteNumberTwo(string id)
        {
            return "RouteNumberTwo" + id;
        }

        [HttpGet("/gettit")]
        public bool Gettit()
        {
            return false;
        }

        public IActionResult NoRouteAttributes()
        {
            return Ok();
        }
    }
}
