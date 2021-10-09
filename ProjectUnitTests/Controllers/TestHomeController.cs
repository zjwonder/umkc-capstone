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
    public class TestHomeController
    {
        private readonly Mock<ILogger<HomeController>> mockLogger = new Mock<ILogger<HomeController>>();
        HomeController controller;

        public TestHomeController()
        {
            controller = new HomeController(mockLogger.Object);
        }


        [Theory]
        [InlineData("Index", "index")]
        public void TestGetIndexView(string viewNameString, string wrongViewNameString)
        {
            var result = controller.Index() as ViewResult;
            Assert.Equal(viewNameString, result.ViewName);
            Assert.NotEqual(wrongViewNameString, result.ViewName);
        }


        [Theory]
        [InlineData("Privacy", "privacy")]
        public void TestGetPrivacyView(string viewNameString, string wrongViewNameString)
        {
            var result = controller.Privacy() as ViewResult;
            Assert.Equal(viewNameString, result.ViewName);
            Assert.NotEqual(wrongViewNameString, result.ViewName);
        }

    }
}
