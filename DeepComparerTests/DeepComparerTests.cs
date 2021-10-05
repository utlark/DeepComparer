using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DeepComparer.Tests
{
    class Base
    {
        public string StringEqual { get; set; }
        public string StringNotEqual { get; set; }
        public string StringNull { get; set; }
        private string Private { get; set; }

        private int setOnly;
        public int SetOnly 
        {
            set
            {
                setOnly = value;
            }       
        }
        public string GetOnly { get; }
        public Level Level { get; set; }      

        public Base() 
        {
            Private = "A";
            GetOnly = "B";
        }
    }
    class Level
    {
        public double DoubleEqual { get; set; }
        public double DoubleNotEqual { get; set; }
        public double DoubleNull { get; set; }
        public Level2 Level2 { get; set; }
    }
    class Level2
    {
        public int IntEqual { get; set; }
        public int IntNotEqual { get; set; }
        public int IntNull { get; set; }
    }

    [TestClass()]
    public class DeepComparerTests
    {
        [TestMethod()]
        public void CompareTest()
        {
            // arrange 
            var comparer = new DeepComparer();
            var first = new Base
            {
                StringEqual = "Equal",
                StringNotEqual = "String",
                StringNull = "NotNull",
                SetOnly = 20,
                Level = new Level
                {
                    DoubleEqual = 5.5,
                    DoubleNotEqual = 10.8,
                    DoubleNull = 15.5,
                    Level2 = new Level2 
                    {
                        IntEqual = 100,
                        IntNotEqual = 250,
                        IntNull = 9
                    }
                }
            };
            var second = new Base
            {
                StringEqual = "Equal",
                StringNotEqual = "NotEqual",
                SetOnly = 25,
                Level = new Level
                {
                    DoubleEqual = 5.5,
                    DoubleNotEqual = 11.7,
                    Level2 = new Level2
                    {
                        IntEqual = 100,
                        IntNotEqual = 175
                    }
                }
            };
            var expected = new List<Difference>
            {
                new Difference("StringNotEqual", first.StringNotEqual, second.StringNotEqual),
                new Difference("StringNull", first.StringNull, null),
                new Difference("Level.DoubleNotEqual", first.Level.DoubleNotEqual, second.Level.DoubleNotEqual),
                new Difference("Level.DoubleNull", first.Level.DoubleNull, Activator.CreateInstance(typeof(double))),
                new Difference("Level.Level2.IntNotEqual", first.Level.Level2.IntNotEqual, second.Level.Level2.IntNotEqual),
                new Difference("Level.Level2.IntNull", first.Level.Level2.IntNull, Activator.CreateInstance(typeof(int)))
            }.ToArray();

            // act
            var result = comparer.Compare<Base>(first, second).ToArray();

            // assert
            bool pass = true;
            for (int i = 0; i < expected.Length; i++)
            {
                pass = pass && Equals(result[i].Path, expected[i].Path) &&
                    Equals(result[i].First, expected[i].First) &&
                    Equals(result[i].Second, expected[i].Second);
            }

            Assert.IsTrue(pass);
        }
    }
}