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

        [HttpPatch]
        public int Patch([FromBody]TestDto dto)
        {
            return 1;
        }

        [HttpOptions]
        public int Options()
        {
            return 1;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TestDto dto)
        {
            return Ok();
        }

        [HttpPost("{someId}")]
        public IActionResult PostWithoutBody(int someId)
        {
            return Ok();
        }

        [HttpPatch("{someId}")]
        public IActionResult PatchWithoutBody(int someId)
        {
            return Ok();
        }

        [HttpPut("{someId}")]
        public IActionResult PutWithoutBody(int someId)
        {
            return Ok();
        }
    }
}
