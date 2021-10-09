using System;
using Xunit;
using CommerceBankProject.Controllers;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using CommerceBankProject.Data;
using Microsoft.EntityFrameworkCore;
using CommerceBankProject.Models;
using System.Linq;

namespace ProjectUnitTests
{
    public class TestNotificationsController
    {
        //this statement is presumably where the error is, need to pass DbContextOptions as a parameter in the creation of the mock
        private readonly Mock<CommerceBankDbContext> mockDbContext = new Mock<CommerceBankDbContext>();
        NotificationsController controller;

        public TestNotificationsController()
        {
            
            //tempRecord.actID = "123456789";
            //tempRecord.actType = "Customer";
            //mockDbContext.Object.Account.Add(tempRecord);

            

            //mockDbContext.SetupAllProperties();

            //mockDbContext.Setup(x => x.Account=tempRecord);
            //mockDbContext.Setup(x => x.Notification.Add(tempNotif));
            
        }

        [Theory]
        [InlineData("Create", "create")]
        public void TestGetCreateView(string viewNameString, string wrongViewNameString)
        {
            controller = new NotificationsController(mockDbContext.Object);
            var result = controller.Create() as ViewResult;
            Assert.Equal(viewNameString, result.ViewName);
            Assert.NotEqual(wrongViewNameString, result.ViewName);
        }

        [Theory]
        [InlineData(123456789, null)]
        public void TestGetDetailsView(int id, int? badID)
        {
            controller = new NotificationsController(mockDbContext.Object);
            var result = controller.Details(id);
            Assert.NotNull(result);

            var notFoundResult = controller.Details(badID);
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Theory]
        [InlineData(123456789, null)]
        public void TestGetEditView(int? id, int? badID)
        {
            controller = new NotificationsController(mockDbContext.Object);
            var result = controller.Edit(id);
            Assert.NotNull(result);

            var notFoundResult = controller.Edit(badID);
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public async void TestGetIndexView()
        {
            Notification tempNotif = new Notification()
            {
                ID = 123456789,
                customerID = "123456789",
                type = "test",
                description = "test description",
                onDate = new DateTime(2008, 1, 20),
                read = false,
                saved = false
            };
            
            mockDbContext.Object.Notification.Add(tempNotif);
            //mockDbContext.Object.SaveChanges();
            //await mockDbContext.SaveChangesAsync();

            mockDbContext.Setup(x => x.Notification.FirstOrDefault()).Returns(tempNotif);

            controller = new NotificationsController(mockDbContext.Object);

            var result = await controller.Index() as ViewResult;
            Assert.IsType<ViewResult>(result);
        }
    }
}