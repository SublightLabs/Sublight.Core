using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sublight.Core;

namespace Sublight.UnitTest
{
    [TestClass]
    public class Api
    {
        [TestMethod]
        public void Login()
        {
            var res = RestApi.LogIn().Result;
            if (res.Value != Guid.Empty && res.ErrorMessage != "No client info.")
            {
                Assert.Fail("Login should fail with 'No client info.' error message.");
            }
        }
    }
}
