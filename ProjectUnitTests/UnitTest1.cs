using System;
using Xunit;

namespace ProjectUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            CommerceBankProject.Models.AccountRecord test1 = new CommerceBankProject.Models.AccountRecord();

            test1.actID = "testid";

            Assert.Equal("testid", test1.actID);
        }

        [Fact]
        public void Test2()
        {
            CommerceBankProject.Models.AccountRecord test1 = new CommerceBankProject.Models.AccountRecord();

            test1.actType = "testType";

            Assert.Equal("testType", test1.actType);
        }

        [Fact]
        public void Test3()
        {
            CommerceBankProject.Models.DateRecord test1 = new CommerceBankProject.Models.DateRecord();

            test1.onDate = new DateTime(2008, 5, 1, 8, 30, 52);

            Assert.Equal(new DateTime(2008, 5, 1, 8, 30, 52), test1.onDate);
        }
    }
}
