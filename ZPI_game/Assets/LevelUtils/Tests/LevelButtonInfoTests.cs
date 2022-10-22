using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LevelUtils
{
    [TestFixture]
    public class LevelButtonInfoTests
    {
        class TestCase
        {
            public readonly LevelButtonInfo lvlButtonInfo;
            public readonly bool isAvailable;
            public TestCase(LevelButtonInfo lvlButtonInfo, bool isAvailable)
            {
                this.lvlButtonInfo = lvlButtonInfo;
                this.isAvailable = isAvailable;
            }
        }
        [Test]
        public static void IsAvailableTest()
        {
            List<TestCase> testCases = new List<TestCase>()
            {
                new TestCase(new LevelButtonInfo("some obj","some obj", 1, true, null), true),
                new TestCase(new LevelButtonInfo("other obj","other obj", 2, true, new List<LevelButtonInfo>()), true),
                new TestCase(new LevelButtonInfo("other obj","other obj", 2, true, new List<LevelButtonInfo>(){new LevelButtonInfo("ga 1", "ga 1", 2, false, null), new LevelButtonInfo("ga 2", "ga 2", 24, false, null), new LevelButtonInfo("ga 3", "ga 3", 46, false, null)}), false),
                new TestCase(new LevelButtonInfo("other obj","other obj", 2, true, new List<LevelButtonInfo>(){new LevelButtonInfo("ga 756", "ga 756", 35, false, null), new LevelButtonInfo("ga 2", "ga 2", 24, true, null), new LevelButtonInfo("ga 97", "ga 97", 73, false, null)}), true),
                new TestCase(new LevelButtonInfo("other obj","other obj", 2, true, new List<LevelButtonInfo>(){new LevelButtonInfo("ga 53", "ga 53", 87, true, null), new LevelButtonInfo("ga 20", "ga 20", 37, true, null)}), true)
            };
            foreach(TestCase testCase in testCases)
            {
                Assert.AreEqual(testCase.isAvailable, testCase.lvlButtonInfo.IsAvailable());
            }
        }
        [Test]
        public static void EqualityTest()
        {
            LevelButtonInfo first = new LevelButtonInfo("ga 1", "ga 1", 1, true, null);
            LevelButtonInfo second = new LevelButtonInfo("ga 1", "ga 1", 1, true, null);
            Assert.AreEqual(first, second);
            first = new LevelButtonInfo("ga 1", "ga 1", 1, true, new List<LevelButtonInfo>());
            second = new LevelButtonInfo("ga 1", "ga 1", 1, true, null);
            Assert.AreNotEqual(first, second);
            first = new LevelButtonInfo("ga 1", "ga 1", 1, true, new List<LevelButtonInfo>());
            second = new LevelButtonInfo("ga 1", "ga 1", 1, true, new List<LevelButtonInfo>());
            Assert.AreEqual(first, second);
            first = new LevelButtonInfo("ga 1", "ga 1", 1, true, new List<LevelButtonInfo>() { new LevelButtonInfo("ga aux", "ga aux", 2, true, null)});
            second = new LevelButtonInfo("ga 1", "ga 1", 1, true, new List<LevelButtonInfo>());
            Assert.AreNotEqual(first, second);
            first = new LevelButtonInfo("ga 1", "ga 1", 1, true, new List<LevelButtonInfo>() { new LevelButtonInfo("ga aux", "ga aux", 2, true, null), new LevelButtonInfo("ga aux 2", "ga aux 2", 3, false, null), new LevelButtonInfo("ga aux 3", "ga aux 3", 13, true, null) });
            second = new LevelButtonInfo("ga 1", "ga 1", 1, true, new List<LevelButtonInfo>() { new LevelButtonInfo("ga aux", "ga aux", 2, true, null), new LevelButtonInfo("ga aux 2", "ga aux 2", 3, false, null), new LevelButtonInfo("ga aux 3", "ga aux 3", 13, true, null) });
            Assert.AreEqual(first, second);
        }
    }
}

