using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sublight.Core;
using Sublight.Core.Types;

namespace Sublight.UnitTest
{
    [TestClass]
    public class ApiSandbox : BaseTest
    {
        public ApiSandbox()
        {
            SetSandboxMode(true);
        }



        [TestMethod]
        public void LoginRegisteredClient()
        {
            InitializeTestClient();

            var res = RestApi.LogIn(TEST_USERNAME, TEST_PASSWORD).Result;

            if (res.Status != Result<Guid>.ResultStatus.Success)
            {
                Assert.Fail("Method should return success in sandbox");
            }

            if (res.Value != DummyGuid)
            {
                Assert.Fail("Expected session is: {0}", DummyGuid);
            }
        }

        [TestMethod]
        public void LoginRegisteredClientWrongUsernamePassword()
        {
            InitializeTestClient();

            var res = RestApi.LogIn("WrongUsername", "WrongPassword").Result;

            if (res.Status == Result<Guid>.ResultStatus.Success)
            {
                Assert.Fail("Method should fail due to invalid credentials");
            }

            if (res.Value != Guid.Empty)
            {
                Assert.Fail("Method should not return session");
            }

            if (res.ErrorMessage != "WrongUsernameOrPassword")
            {
                Assert.Fail("WrongUsernameOrPassword error expected");
            }
        }

        [TestMethod]
        public void LoginUnregisteredClient()
        {
            Globals.API_CLIENT_ID = "UNKNOWN_CLIENT_ID";
            Globals.API_CLIENT_KEY = "UNKNOWN_CLIENT_KEY";

            var res = RestApi.LogIn(TEST_USERNAME, TEST_PASSWORD).Result;

            if (res.Status == Result<Guid>.ResultStatus.Success)
            {
                Assert.Fail("Login should fail with unregistered client");
            }

            if (res.ErrorMessage != "ClientNotSupported")
            {
                Assert.Fail("ClientNotSupported error expected");
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
