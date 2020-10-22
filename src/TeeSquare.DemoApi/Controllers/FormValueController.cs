using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TeeSquare.DemoApi.Controllers
{
    public class FormValueController : Controller
    {
        [HttpPost]
        public int PostSomeValues([FromForm] string name, [FromForm] IFormFile specialFile)
        {
            return 0;
        }

    }
}
