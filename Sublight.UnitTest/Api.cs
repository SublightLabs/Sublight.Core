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

        [TestMethod]
        public void LogoutUnknownSession()
        {
            var res = RestApi.LogOut(new Guid("5829DF78-87E4-43BA-A999-9C0ECC58F311")).Result;
            if (res.ErrorMessage == null || res.ErrorMessage.IndexOf("active session not found", StringComparison.Ordinal) < 0)
            {
                Assert.Fail("Active session not found error expected");
            }
        }
    }
}
