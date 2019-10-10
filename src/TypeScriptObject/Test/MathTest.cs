using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TypeScript.CSharp.Tests
{
    [TestClass]
    public class MathTest : TestBase
    {
        [TestMethod]
        public void AbsTest()
        {
            Assert.AreEqual<Number>(1, Math.abs(1));
            Assert.AreEqual<Number>(3.2, Math.abs(3.2));
            Assert.AreEqual<Number>(1, Math.abs(-1));
            Assert.AreEqual<Number>(3.2, Math.abs(-3.2));
            Assert.AreEqual<Number>(0, Math.abs(null));
        }

        [TestMethod]
        public void MaxTest()
        {
            Assert.AreEqual(Number.NEGATIVE_INFINITY, Math.max());
            Assert.AreEqual<Number>(1, Math.max(1));
            Assert.AreEqual<Number>(3, Math.max(1, 2, 3));
            Assert.AreEqual<Number>(1, Math.max(-1, 0, 1));
        }

        [TestMethod]
        public void MinTest()
        {
            Assert.AreEqual(Number.POSITIVE_INFINITY, Math.min());
            Assert.AreEqual<Number>(1, Math.min(1));
            Assert.AreEqual<Number>(1, Math.min(1, 2, 3));
            Assert.AreEqual<Number>(-1, Math.min(-1, 0, 1));
        }
    }
}
