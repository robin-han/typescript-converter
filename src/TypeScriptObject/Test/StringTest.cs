using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace TypeScript.CSharp.Tests
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

        [TestMethod]
        public void ReplaceTest()
        {
            String input = "abc12345#$*%";
            String newString = input.replace(new RegExp("([^\\d]*)(\\d*)([^\\w]*)"), (match, p1, p2, p3) =>
            {
                return string.Join(" - ", p1, p2, p3);
            });

            Assert.AreEqual<String>("abc - 12345 - #$*%", newString);
        }

        [TestMethod]
        public void RegExecTest()
        {
            var regex = new RegExp("quick\\s(brown).+?(jumps)", "i");
            var result = regex.exec("The Quick Brown Fox Jumps Over The Lazy Dog");

            Assert.AreEqual<string>("Quick Brown Fox Jumps", result[0]);
            Assert.AreEqual<string>("Brown", result[1]);
            Assert.AreEqual<string>("Jumps", result[2]);
        }

        [TestMethod]
        public void MatchTest()
        {
            String text = "table football, foosball";
            RegExp regex = new RegExp("foo*");
            RegExpArray result = text.match(regex);
            Assert.AreEqual<int>(1, (int)result.length);
            Assert.AreEqual<string>("foo", result[0]);

            regex = new RegExp("foo*", "g");
            result = text.match(regex);
            Assert.AreEqual<int>(2, (int)result.length);
            Assert.AreEqual<string>("foo", result[0]);
            Assert.AreEqual<string>("foo", result[1]);
        }

        [TestMethod]
        public void SplitTest()
        {
            String text = "table football, foosball";
            RegExp regex = new RegExp("foo*");
            Array<String> result = text.split(regex);
            Assert.AreEqual<int>(3, (int)result.length);
            Assert.AreEqual<string>("table ", result[0]);
            Assert.AreEqual<string>("tball, ", result[1]);
            Assert.AreEqual<string>("sball", result[2]);

            regex = new RegExp("foo*", "g");
            result = text.split(regex);
            Assert.AreEqual<int>(3, (int)result.length);
            Assert.AreEqual<string>("table ", result[0]);
            Assert.AreEqual<string>("tball, ", result[1]);
            Assert.AreEqual<string>("sball", result[2]);
        }

        [TestMethod]
        public void SearchTest()
        {
            String text = "table football, foosball";
            RegExp regex = new RegExp("foo*");
            Number result = text.search(regex);
            Assert.AreEqual(6, result);

            regex = new RegExp("foo*", "g");
            result = text.search(regex);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void ReplaceTest01()
        {
            String text = "table football, foosball";
            RegExp regex = new RegExp("foo*");
            String result = text.replace(regex, "T");
            Assert.AreEqual<string>("table Ttball, foosball", result);

            regex = new RegExp("foo*", "g");
            result = text.replace(regex, "T");
            Assert.AreEqual<string>("table Ttball, Tsball", result);
        }

        [TestMethod]
        public void ReplaceTest02()
        {
            String text = "table football, foosball";
            RegExp regex = new RegExp("foo*");
            String result = text.replace(regex, (match, p1) =>
            {
                return "T";
            });
            Assert.AreEqual<string>("table Ttball, foosball", result);

            regex = new RegExp("foo*", "g");
            int i = 0;
            result = text.replace(regex, (match, p1) =>
            {
                return "T" + i++;
            });
            Assert.AreEqual<string>("table T0tball, T1sball", result);
        }

    }
}
