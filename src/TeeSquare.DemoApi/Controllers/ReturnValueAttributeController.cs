using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace TeeSquare.DemoApi.Controllers
{
    [Route("api/return-test")]
    public class ReturnValueAttributeController: Controller
    {
        [ProducesResponseType(typeof(int), 200)]
        public IActionResult Test()
        {
            return new JsonResult(12);
        }
    }
}
