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

        [Fact]
        public void TestGetIndexView()
        {
            string viewNameString = "Index", wrongViewNameString = "index";
            var result = controller.Index() as ViewResult;
            Assert.Equal(viewNameString, result.ViewName);
            Assert.NotEqual(wrongViewNameString, result.ViewName);
        }

        [Fact]
        public void TestGetPrivacyView()
        {
            string viewNameString = "Privacy", wrongViewNameString = "privacy";
            var result = controller.Privacy() as ViewResult;
            Assert.Equal(viewNameString, result.ViewName);
            Assert.NotEqual(wrongViewNameString, result.ViewName);
        }
    }
}
