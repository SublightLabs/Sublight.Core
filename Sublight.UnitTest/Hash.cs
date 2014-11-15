using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sublight.Core;
using Sublight.Core.Types;

namespace Sublight.UnitTest
{
    [TestClass]
    public class Hash
    {
        [TestMethod]
        public void HashFromDummyFile()
        {
            var res = HashUtility.CalculateHashAsync(@".\TestFiles\DummyTestFile.avi").Result;
            if (res == null || res.Status != Result<string>.ResultStatus.Success || string.IsNullOrWhiteSpace(res.Value))
            {
                Assert.Fail("Hash calculation failed!");
            }

            if (res.Value != "02000000024ab38c2e3e2cc4627bcd4e2cfc4049")
            {
                Assert.Fail("Wrong hash value was calculated!");
            }
        }

        [TestMethod]
        public void HashFromDummyStream()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                for (int i = 0; i < 1024*128; i++)
                {
                    ms.WriteByte((byte)(i % 256));
                }

                var res = HashUtility.CalculateHashAsync(ms).Result;
                if (res == null || res.Status != Result<string>.ResultStatus.Success || string.IsNullOrWhiteSpace(res.Value))
                {
                    Assert.Fail("Hash calculation failed!");
                }

                if (res.Value != "02000000020000f20e9a677287e0abf20e9a678a")
                {
                    Assert.Fail("Wrong hash value was calculated!");
                }
            }
        }
    }
}
