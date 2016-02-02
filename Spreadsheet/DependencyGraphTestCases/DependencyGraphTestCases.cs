using System;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyGraphTestCases
{
    [TestClass]
    public class DependencyGraphTestCases
    {
        /// <summary>
        /// Generic test of constructor and size comparison
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            DependencyGraph g = new DependencyGraph();
            DependencyGraph f = new DependencyGraph();
            g.AddDependency("1","a");
            f.AddDependency("z","b");
            Assert.AreEqual(f.Size,g.Size,.0001);
        }

        /// <summary>
        /// Testing size at 0
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.AreEqual(0, g.Size, .0001);
        }

        /// <summary>
        /// Generic test of hasDependees when it doesn't have any
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1","2");
            Assert.IsFalse(g.HasDependees("1"));
        }

        /// <summary>
        /// Generic test of hasDependees when it does have some
        /// </summary>
        [TestMethod]
        public void TestMethod4()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            Assert.IsTrue(g.HasDependees("2"));
        }

        /// <summary>
        /// Generic test of hasDependents when it has some
        /// </summary>
        [TestMethod]
        public void TestMethod5()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            Assert.IsTrue(g.HasDependents("1"));
        }

        /// <summary>
        /// Generic test of hasDependents when it doesn't have any
        /// </summary>
        [TestMethod]
        public void TestMethod6()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            Assert.IsFalse(g.HasDependents("2"));
        }

        /// <summary>
        /// Generic test of removedependency
        /// </summary>
        [TestMethod]
        public void TestMethod7()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            g.RemoveDependency("1", "2");
            Assert.AreEqual(0, g.Size, .0001);
        }

        /// <summary>
        /// A size test when they are all dependents of one thing
        /// </summary>
        [TestMethod]
        public void TestMethod8()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("2", "1");
            g.AddDependency("2", "2");
            g.AddDependency("2", "3");
            Assert.AreEqual(3,g.Size,.001);
        }

        /// <summary>
        /// Test of remove dependency and size
        /// </summary>
        [TestMethod]
        public void TestMethod9()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("2", "1");
            g.AddDependency("2", "2");
            g.AddDependency("2", "3");
            g.RemoveDependency("2","2");
            Assert.AreEqual(2, g.Size, .001);
        }

        /// <summary>
        /// Test of get dependents
        /// </summary>
        [TestMethod]
        public void TestMethod10()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("2", "1");
            g.AddDependency("2", "3");
            g.AddDependency("2", "2");
            foreach (string s in g.GetDependents("2"))
            {
                Assert.IsTrue(s == "1" || s == "2" || s == "3");
            }
        }

        /// <summary>
        /// Test of get dependents after removing a dependent
        /// </summary>
        [TestMethod]
        public void TestMethod11()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("2", "1");
            g.AddDependency("2", "2");
            g.AddDependency("2", "3");
            g.RemoveDependency("2","2");
            string test = "";
            foreach (string s in g.GetDependents("2"))
            {
                test = test + s;
            }
            Assert.IsTrue(test == "13");
        }

        /// <summary>
        /// Test of get dependees
        /// </summary>
        [TestMethod]
        public void TestMethod12()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            g.AddDependency("2", "2");
            g.AddDependency("3", "2");
            string test = "";
            foreach (string s in g.GetDependees("2"))
            {
                test = test + s;
            }
            Assert.IsTrue(test == "123");
        }

        /// <summary>
        /// Test of get dependees after removing a dependency
        /// </summary>
        [TestMethod]
        public void TestMethod13()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            g.AddDependency("2", "2");
            g.AddDependency("3", "2");
            g.RemoveDependency("2", "2");
            string test = "";
            foreach (string s in g.GetDependees("2"))
            {
                test = test + s;
            }
            Assert.IsTrue(test == "13");
        }

        /// <summary>
        /// Test of get dependees with null
        /// </summary>
        [TestMethod]
        public void TestMethod14()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            string test = "";
            foreach (string s in g.GetDependees(null))
            {
                test = test + s;
            }
        }

        /// <summary>
        /// Test of get dependents with null
        /// </summary>
        [TestMethod]
        public void TestMethod15()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            string test = "";
            foreach (string s in g.GetDependents(null))
            {
                test = test + s;
            }
        }

        /// <summary>
        /// Test of add dependency with null
        /// </summary>
        [TestMethod]
        public void TestMethod16()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", null);
        }

        /// <summary>
        /// Test of add dependency with null
        /// </summary>
        [TestMethod]
        public void TestMethod16a()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency(null, "l");
        }

        /// <summary>
        /// Test of remove dependency with null
        /// </summary>
        [TestMethod]
        public void TestMethod17()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            g.RemoveDependency(null, "2");
        }

        /// <summary>
        /// Test of remove dependency with null
        /// </summary>
        [TestMethod]
        public void TestMethod17a()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            g.RemoveDependency("l", null);
        }

        /// <summary>
        /// Test of has dependees with null
        /// </summary>
        [TestMethod]
        public void TestMethod18()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            g.HasDependees(null);
        }

        /// <summary>
        /// Test of has dependents with null
        /// </summary>
        [TestMethod]
        public void TestMethod19()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            g.HasDependents(null);
        }

        /// <summary>
        /// Test of replace dependents
        /// </summary>
        [TestMethod]
        public void TestMethod20()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("2", "1");
            g.AddDependency("2", "2");
            g.AddDependency("2", "3");

            string[] test = { "a", "b", "c", "d" };

            g.ReplaceDependents("2",test);

            string ans = "";

            foreach (string s in g.GetDependents("2"))
            {
                ans = ans + s;
            }
            Assert.IsTrue(ans == "abcd");
        }

        /// <summary>
        /// Test of replace dependents with null
        /// </summary>
        [TestMethod]
        public void TestMethod21()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("2", "1");
            g.AddDependency("2", "2");
            g.AddDependency("2", "3");

            string[] test = { "a", "b", "c", "d" };

            g.ReplaceDependents(null, test);

            string ans = "";

            foreach (string s in g.GetDependents("2"))
            {
                ans = ans + s;
            }
        }

        /// <summary>
        /// Test of replace dependents with null
        /// </summary>
        [TestMethod]
        public void TestMethod22()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("2", "1");
            g.AddDependency("2", "2");
            g.AddDependency("2", "3");

            string[] test = { "a", "b", "c", "d" };

            g.ReplaceDependents("2", null);

            string ans = "";

            foreach (string s in g.GetDependents("2"))
            {
                ans = ans + s;
            }
        }

        /// <summary>
        /// Test of replace dependents with a non exsistant key
        /// </summary>
        [TestMethod]
        public void TestMethod23()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("2", "1");
            g.AddDependency("2", "2");
            g.AddDependency("2", "3");

            string[] test = { "a", "b", "c", "d" };

            g.ReplaceDependents("@", test);

            string ans = "";

            foreach (string s in g.GetDependents("2"))
            {
                ans = ans + s;
            }
            Assert.IsTrue(ans == "123");
        }

        /// <summary>
        /// Test of replace dependents with less new dependents
        /// </summary>
        [TestMethod]
        public void TestMethod24()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("2", "1");
            g.AddDependency("2", "2");
            g.AddDependency("2", "3");

            string[] test = { "a", "b"};

            g.ReplaceDependents("2", test);

            string ans = "";

            foreach (string s in g.GetDependents("2"))
            {
                ans = ans + s;
            }
            Assert.IsTrue(ans == "ab");
        }

        /// <summary>
        /// Test of replace dependees
        /// </summary>
        [TestMethod]
        public void TestMethod25()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            g.AddDependency("2", "2");
            g.AddDependency("3", "2");

            string[] test = { "a", "b", "c", "d" };

            g.ReplaceDependees("2", test);

            string ans = "";

            foreach (string s in g.GetDependents("a"))
            {
                ans = ans + s;
            }
            Assert.IsTrue(ans == "2");
        }

        /// <summary>
        /// Test of replace dependees
        /// </summary>
        [TestMethod]
        public void TestMethod26()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("1", "2");
            g.AddDependency("2", "2");
            g.AddDependency("3", "2");

            string[] test = { "a", "b", "c", "d" };

            g.ReplaceDependees("2", test);

            string ans = "";

            foreach (string s in g.GetDependents("1"))
            {
                ans = ans + s;
            }
            Assert.IsTrue(ans == "");
        }
    }
}
