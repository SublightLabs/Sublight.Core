using System;
using Sublight.Core;

namespace Sublight.UnitTest
{
    public class BaseTest
    {
        protected void SetSandboxMode(bool isSandbox)
        {
            Globals.IsSandboxMode = isSandbox;
        }

        protected void InitializeTestClient()
        {
            Globals.API_CLIENT_ID = "YOUR_CLIENT_ID";
            Globals.API_CLIENT_KEY = "YOUR_CLIENT_KEY";
        }

        protected static readonly Guid DummyGuid = new Guid("79492A46-F0C2-41C4-BC9A-F79AEEDC1C65");
        protected const string TEST_USERNAME = "TestUser";
        protected const string TEST_PASSWORD = "TestPassword";
    }
}
