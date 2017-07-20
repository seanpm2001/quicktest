﻿using DemoApp;
using NUnit.Framework;
using QuickTest;

namespace Tests
{
    public class ListViewTests : IntegrationTest<App>
    {
        [SetUp]
        protected override void SetUp()
        {
            base.SetUp();

            OpenMenu("ListViews");
            ShouldSee("ListView demo");
        }

        [Test]
        public void TestTextCell()
        {
            Tap("Item A1");
            ShouldSee("Item A1 tapped");
        }

        [Test]
        public void TestStringViewCell()
        {
            Tap("Item A2");
            ShouldSee("Item A2 tapped");
        }

        [Test]
        public void TestItemViewCell()
        {
            Tap("Item A3");
            ShouldSee("Item A3 tapped");
        }

        [Test]
        public void TestGroups()
        {
            Tap("Item A4");
            ShouldSee("Item A4 tapped");
        }

        protected override void TearDown()
        {
            Tap("Ok");
            ShouldSee("ListView demo");

            base.TearDown();
        }
    }
}
