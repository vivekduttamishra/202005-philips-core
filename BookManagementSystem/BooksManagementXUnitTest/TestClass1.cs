using System;
using Xunit;

namespace BooksManagementXUnitTest
{
    //[TestClass]
    public class TestClass1
    {
        //No [TestInitialize] --> Use Constructor
        int x;

        public TestClass1()
        {
            //Do you Test initializations here!
            x = 10;
        }

        //No [TestFinalize] --> Use Dispose method test cleanup here your test

        public void Dispose()
        {
            //Do your test Cleanup Here!
        }

        [Fact] //->[TestMethod]
        public void Test1()
        {
            Assert.Equal(10, x);
        }

        [Fact]
        public void FailingTest()
        {
            Assert.True(false,"Because This test is mean to fail"); //alternative of Fail in other frameworks
        }

        //[Ignore]  //---> repalced with Skip property in fact
        [Fact(Skip ="Just Testing Skip")]
        public void IgnoredTest()
        {
            Assert.True(false, "This should never come as this is ignored"); //alternative of Fail in other frameworks
        }
    }
}
