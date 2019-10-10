using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeScript.CSharp.Tests
{
    [TestClass]
    public class UndefinedTest: TestBase
    {
        [TestMethod]
        public void NormalTest()
        {
            Undefined un = Undefined.Value;

            Assert.IsFalse(un);
            Assert.IsTrue(un == null);
            Assert.IsFalse(un != null);
            Assert.IsTrue(undefined == un);
            Assert.IsFalse(undefined != un);
        }
    }
}
