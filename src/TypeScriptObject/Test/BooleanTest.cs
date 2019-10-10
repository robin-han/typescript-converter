using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeScript.CSharp.Tests
{
    [TestClass]
    public class BooleanTest : TestBase
    {
        [TestMethod]
        public void NormalTest()
        {
            Boolean b = true;
            Assert.IsTrue(b);

            b = false;
            Assert.IsFalse(b);
        }

        [TestMethod]
        public void NullUndefinedTest()
        {
            Boolean c = null;
            Assert.IsFalse(c);

            c = undefined;
            Assert.IsFalse(c);

            Boolean a = undefined;
            Boolean b = undefined;
            Assert.IsTrue(a == b);

            a = null;
            b = undefined;
            Assert.IsTrue(a == b);

            a = undefined;
            b = null;
            Assert.IsTrue(a == b);

            a = null;
            b = null;
            Assert.IsTrue(a == b);

            a = null;
            b = true;
            Assert.IsFalse(a == b);

            a = false;
            b = null;
            Assert.IsFalse(a == b);

            a = undefined;
            b = true;
            Assert.IsFalse(a == b);

            a = false;
            b = undefined;
            Assert.IsFalse(a == b);
        }
    }
}
