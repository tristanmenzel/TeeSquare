using Microsoft.AspNetCore.Mvc;

namespace TeeSquare.DemoApi.Controllers
{
    [Controller]
    public class DtoFromRouteOrQueryController: Controller
    {
        public class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [HttpGet("from-route/{id}/{name}")]
        public TestObject FromRoute([FromRoute] TestObject obj)
        {
            return obj;
        }

        [HttpGet("from-query")]
        public TestObject FromQuery([FromQuery] TestObject obj)
        {
            return obj;
        }

        public TestObject FromQuery([FromQuery] int[] numbers)
        {
            return new TestObject();
        }
    }
}
