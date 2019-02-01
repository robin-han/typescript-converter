using Microsoft.VisualStudio.TestTools.UnitTesting;
using GrapeCity.DataVisualization.TypeScript;

namespace GrapeCity.DataVisualization.UnitTest.TypeScript
{
    [TestClass]
    public class StringTest : TestBase
    {
        [TestMethod]
        public void NormalTest()
        {
            String s1 = "abc";
            String s2 = "abc";
            Assert.IsTrue("abc" == s1);

            String s3 = undefined;
            Assert.IsTrue(s3 == undefined);
            Assert.AreEqual(s3, undefined);
            Assert.AreEqual(undefined, s3);

            String s4 = null;
            Assert.IsTrue(s4 == null);
        }

        [TestMethod]
        public void NullUndefinedTest()
        {
            String a = undefined;
            String b = undefined;
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
            b = "";
            Assert.IsFalse(a == b);

            a = "";
            b = null;
            Assert.IsFalse(a == b);

            a = undefined;
            b = "";
            Assert.IsFalse(a == b);

            a = "";
            b = undefined;
            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void BooleanTest()
        {
            String s1 = "123";
            Assert.IsTrue(s1);

            dynamic d = s1;
            Assert.IsTrue(d);

            String s2 = "";
            Assert.IsFalse(s2);


        }

        [TestMethod]
        public void ConcatTest()
        {
            String s1 = "abc";
            String s2 = "def";
            Assert.AreEqual<String>("abcdef", s1 + s2);
        }
    }
}
