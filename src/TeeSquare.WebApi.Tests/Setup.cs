using NUnit.Framework;
using TeeSquare.WebApi.Core31;

namespace TeeSquare.WebApi.Tests
{
    [SetUpFixture]
    public class TestsSetupClass
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            Core31Configurator.Configure();
        }
    }
}
