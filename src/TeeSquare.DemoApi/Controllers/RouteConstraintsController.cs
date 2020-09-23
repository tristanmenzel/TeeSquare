using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TeeSquare.DemoApi.Dtos;

namespace TeeSquare.DemoApi.Controllers
{
    [Route("api/[controller]")]
    public class RouteConstraintsController : Controller
    {
        [HttpGet("user/{id:int}")]
        public string GetUserById(int id)
            => $"User {id}";

        [HttpGet]
        [Route("user/{name:regex(.*)}")]
        public string[] GetUsersByName(string name, [FromQuery] int? limit)
            => new[] { "User1", "User2" }.Take(limit ?? 99).ToArray();

        [HttpGet("user/{name}/{page:range(1, 999999)}/{pageSize:int:min(23)}")]
        public string[] GetUsersByName(string name, int page, int pageSize)
            => new[] { "User1", "User2" };

        [HttpPut("user/{age:int:min(1):max(199)}")]
        public IActionResult AddUser(int age, [FromBody] TestDto dto)
            => Ok();
    }
}
