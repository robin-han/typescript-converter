using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TypeScript.CSharp.Tests
{
    [TestClass]
    public class DateTest : TestBase
    {

        [TestMethod]
        public void NormalTest()
        {
            Date date = new Date(2019, 0, 29);
            Assert.AreEqual(2019, date.getFullYear());
            Assert.AreEqual(0, date.getMonth());
            Assert.AreEqual(29, date.getDate());
            Assert.AreEqual(2, date.getDay());
        }

        [TestMethod]
        public void NullUndefinedTest()
        {
            Date d = null;
            Assert.IsFalse(d);

            d = undefined;
            Assert.IsFalse(d);

            Date a = undefined;
            Date b = undefined;
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
        }

    }
}
