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

            if (Globals.API_CLIENT_ID == "YOUR_CLIENT_ID" &&
                Globals.API_CLIENT_KEY == "YOUR_CLIENT_KEY")
            {
                if (res.ErrorMessage != "ClientNotSupported")
                {
                    Assert.Fail("ClientNotSupported error expected");
                }
            }
            else if (res.Value == Guid.Empty)
            {
                Assert.Fail(string.Format("Login failed with error: '{0}'", res.ErrorMessage));
            }
        }
    }
}
