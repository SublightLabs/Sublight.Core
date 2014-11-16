using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sublight.Core;

namespace Sublight.UnitTest
{
    [TestClass]
    public class ApiProduction : BaseTest
    {
        public ApiProduction()
        {
            SetSandboxMode(false);
        }

        [TestMethod]
        public void LoginRegisteredClient()
        {
            InitializeTestClient();

            var res = RestApi.LogIn(string.Empty, string.Empty).Result;

            if (res.ErrorMessage != "ClientNotSupported")
            {
                Assert.Fail("ClientNotSupported error expected");
            }
        }
    }
}
