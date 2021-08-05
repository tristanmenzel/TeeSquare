using Microsoft.AspNetCore.Mvc;

namespace TeeSquare.DemoApi.Controllers
{
    [Route("faux-primitives")]
    public class FauxPrimitiveController: Controller
    {
        [HttpGet("/get-something")]
        public int GetSomething(int? realPrimitive, FauxPrimitive? fauxPrimitive)
        {
            return 0;
        }

        public struct FauxPrimitive
        {
            public string Value { get; }

            public FauxPrimitive(string value)
            {
                Value = value;
            }
        }
    }
}
