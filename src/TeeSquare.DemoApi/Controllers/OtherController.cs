using System;
using Microsoft.AspNetCore.Mvc;

namespace TeeSquare.DemoApi.Controllers
{
    [Route("api/other")]
    public class OtherController : Controller
    {
        [HttpGet("do-a-thing")]
        public int DoAThing([FromQuery]DateTime? when)
        {
            return 42;
        }
    }
}