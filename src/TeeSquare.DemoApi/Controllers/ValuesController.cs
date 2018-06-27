using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

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
    
    [Route("api")]
    public class RouteOnActionController : Controller
    {
        [Route("route-number-one")]
        public string RouteNumberOne()
        {
            return "RouteNumberOne";
        }

        [Route("/alt-api/route-number-two/")]
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

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}