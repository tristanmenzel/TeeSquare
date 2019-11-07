using System.Net;
using Microsoft.AspNetCore.Mvc;
using TeeSquare.DemoApi.Dtos;

namespace TeeSquare.DemoApi.Controllers
{
    [Route("api/test")]

    public class TestController: Controller
    {
        [HttpGet("{id}")]
        public TestDto Get(int id)
        {
            return default(TestDto);
        }

        [HttpPost]
        public int Post([FromBody]TestDto dto)
        {
            return 1;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TestDto dto)
        {
            return Ok();
        }
    }
}
