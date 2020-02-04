using NUnit.Framework;
using TeeSquare.WebApi.Core22;

namespace TeeSquare.WebApi.Tests
{
    [SetUpFixture]
    public class TestsSetupClass
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            Core22Configurator.Configure();
        }
    }
}
