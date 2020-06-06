using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace BooksManagementXUnitTest
{
    public class PrimeUtilsTest : IDisposable
    {
        PrimeUtils utils;
        ITestOutputHelper output;
        DateTime Start;
        public PrimeUtilsTest(ITestOutputHelper output)
        {
            this.output = output;
            utils = new PrimeUtils();
            Console.WriteLine("Console: PrimeUtilsTest is created");
            Start = DateTime.Now;
            output.WriteLine("Output: PrimeUtilsTest is created ");
        }

        public void Dispose()
        {
            Console.WriteLine("Console: Dispose is Called");
            var span = DateTime.Now - Start;
            output.WriteLine("Output: Dispose is called --" +span.TotalMilliseconds);
        }

        [Fact]
        public void IsPrime_ReturnsTrueFor2()
        {
            Assert.True(utils.IsPrime(2)," 2 should be prime");
        }

        [Fact]
        public void IsPrime_ReturnsFalseFor5()
        {
            Assert.False(utils.IsPrime(4), " 4shouldn't be prime");
        }

        // [TestMethod]
        //[ExpectedException(typeof(ArithmeticException)] //-->Replaced with Assert.Throws
        //public void IsPrime_ShouldThrowExceptionForNegativeInput()
        //{
        //    utils.IsPrime(-2); //If it throws, your test passes
        //    //I can't assert on the error message or other exception details
        //}


        [Fact]
        public void IsPrime_ShouldThrowExceptionForNegativeInput()
        {
            Assert.Throws<ArithmeticException>(() => utils.IsPrime(-3));
        }

        [Fact]
        public void IsPrime_ShouldThrowExceptionForNegativeInputWithNumberInMessage()
        {
            int testValue = -23;
            //If exception is thrown, you get a reference to exception object
            var ex = Assert
                   .Throws<ArithmeticException>(() => utils.IsPrime(testValue));

            Assert.Contains(testValue.ToString(), ex.Message);
        }

        [InlineData(4,false)]
        [InlineData(7,true)]
        [InlineData(1,false)]
        [InlineData(0,false)]
        [InlineData(9,true)]        
        //[Fact] ---> With Data Driven Design instead of [Fact] we use [Theory]
        [Theory]
        public void IsPrime_ShouldReturnExcpectedResults(int number, bool expectedResult)
        {
            var actualResult = utils.IsPrime(number);

            Assert.True(actualResult == expectedResult, $"IsPrime({number}) should be {expectedResult}. Found {actualResult}");
            

        }

        [MemberData(memberName:nameof(PrimeTestCaseData.PrimeDataList), 
                    MemberType =typeof(PrimeTestCaseData))]
        [Theory]
        public void IsPrime_ShouldReturnExcpectedResultsBasedOnMemberData(object data)
        {
            var primeData = data as PrimeData;
            var actualResult = utils.IsPrime(primeData.Value);

            Assert.True(actualResult == primeData.ExpectedResult, $"IsPrime({primeData.Value}) should be {primeData.ExpectedResult}. Found {actualResult}");


        }

        [Fact]
        public void IsPrimeAsync_CanBeTestedByManualWait()
        {
            int testValue = 13;

            var task = utils.IsPrimeAsync(testValue);

            task.Wait();

            Assert.True(task.Result);
        }

        [Fact]
        public async Task IsPrimeAsync_CanBeTestedAsync()
        {
            int testValue = 13;

            var result = await utils.IsPrimeAsync(testValue);

            Assert.True(result);
        }


    }
}
