using System;
using Xunit;
using CommerceBankProject.Controllers;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using CommerceBankProject.Data;

namespace ProjectUnitTests
{
    public class TestNotificationsController
    {
        //this statement is presumably where the error is, need to pass DbContextOptions as a parameter in the creation of the mock
        private readonly Mock<CommerceBankDbContext> mockDbContext = new Mock<CommerceBankDbContext>();
        NotificationsController controller;

        public TestNotificationsController()
        {
            CommerceBankProject.Models.AccountRecord tempRecord = new CommerceBankProject.Models.AccountRecord();
            tempRecord.actID = "123456789";
            tempRecord.actType = "Customer";
            mockDbContext.Object.Account.Add(tempRecord);
            //mockDbContext.Setup(x => x.Account=tempRecord);
            controller = new NotificationsController(mockDbContext.Object);

        }

        [Theory]
        [InlineData("Create", "create")]
        public void TestGetCreateView(string viewNameString, string wrongViewNameString)
        {
            var result = controller.Create() as ViewResult;
            Assert.Equal(viewNameString, result.ViewName);
            Assert.NotEqual(wrongViewNameString, result.ViewName);
        }
    }
}