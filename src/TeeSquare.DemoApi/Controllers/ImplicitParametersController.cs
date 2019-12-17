using Microsoft.AspNetCore.Mvc;
using TeeSquare.DemoApi.Dtos;

namespace TeeSquare.DemoApi.Controllers
{
    [Route("api/other")]
    public class ImplicitParametersController : Controller
    {
        [HttpGet("implicit-query")]
        public int ImplicitQuery(int id)
        {
            return default;
        }
        [HttpGet("implicit-route/{id}")]
        public int ImplicitRoute(int id)
        {
            return default;
        }
        [HttpPost("implicit-body")]
        public int ImplicitQuery(TestDto dto)
        {
            return default;
        }

    }
}
