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
using static ProjectUnitTests.Utilities.Utilities;
using System.Collections.Generic;

namespace ProjectUnitTests
{
    public class TestNotificationsController
    {
        private readonly Mock<CommerceBankDbContext> mockDbContext = new Mock<CommerceBankDbContext>();
        NotificationsController controller;

        [Fact]
        public void TestGetCreateView()
        {
            string viewNameString = "Create", wrongViewNameString = "create";
            controller = new NotificationsController(mockDbContext.Object);
            var result = controller.Create() as ViewResult;
            Assert.Equal(viewNameString, result.ViewName);
            Assert.NotEqual(wrongViewNameString, result.ViewName);
        }

        [Fact]
        public void TestGetDetailsView()
        {
            int id = 123456789;
            int? badID = null;
            controller = new NotificationsController(mockDbContext.Object);
            var result = controller.Details(id);
            Assert.NotNull(result);

            var notFoundResult = controller.Details(badID);
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public async void TestGetEditView()
        {
            int id = 987654321;
            int? badID = null;
            using (var context = new CommerceBankDbContext(TestDbContextOptions()))
            {
                Notification tempNotif = new Notification()
                {
                    ID = 987654321,
                    customerID = "987654321",
                    type = "test2",
                    description = "test description 2",
                    onDate = new DateTime(2021, 1, 20),
                    read = false,
                    saved = false
                };

                // When

                context.Add(tempNotif);
                context.SaveChanges();
                controller = new NotificationsController(context);

                var result = await controller.Edit(id);
                var viewResult = result as ViewResult;
                Assert.IsType<ViewResult>(result);
                Assert.Equal(viewResult.Model, tempNotif);

                var notFoundResult = controller.Edit(badID);
                Assert.IsType<NotFoundResult>(notFoundResult.Result);
            }
        }

        [Fact]
        public async void TestGetIndexView()
        {
            using (var context = new CommerceBankDbContext(TestDbContextOptions()))
            {
                // Given

                List<Notification> notifications = new List<Notification>
                {
                    new Notification()
                    {
                        ID = 987654321,
                        customerID = "987654321",
                        type = "test2",
                        description = "test description 2",
                        onDate = new DateTime(2021, 1, 20),
                        read = false,
                        saved = false
                    },
                    new Notification()
                    {
                        ID = 123456789,
                        customerID = "123456789",
                        type = "test",
                        description = "test description",
                        onDate = new DateTime(2008, 1, 20),
                        read = false,
                        saved = false
                    }
                };

                // When

                foreach (Notification n in notifications)
                {
                    context.Notification.Add(n);
                }
                context.SaveChanges();
                controller = new NotificationsController(context);

                // Then

                var result = await controller.Index();
                var viewResult = result as ViewResult;
                Assert.IsType<ViewResult>(viewResult);
                Assert.Equal(viewResult.Model, notifications );
            }
        }

        [Fact]
        public async void TestGetDeleteView()
        {
            int id = 987654321;
            int? badID = null;
            using (var context = new CommerceBankDbContext(TestDbContextOptions()))
            {
                Notification tempNotif = new Notification()
                {
                    ID = 987654321,
                    customerID = "987654321",
                    type = "test2",
                    description = "test description 2",
                    onDate = new DateTime(2021, 1, 20),
                    read = false,
                    saved = false
                };

                // When

                context.Add(tempNotif);
                context.SaveChanges();
                controller = new NotificationsController(context);

                var result = await controller.Delete(id);
                var viewResult = result as ViewResult;
                Assert.IsType<ViewResult>(result);
                Assert.Equal(viewResult.Model, tempNotif);

                var notFoundResult = controller.Delete(badID);
                Assert.IsType<NotFoundResult>(notFoundResult.Result);
            }
        }

        [Fact]
        public async void TestCreatePOSTWithValidModelState()
        {
            using (var context = new CommerceBankDbContext(TestDbContextOptions()))
            {
                Notification tempNotif = new Notification()
                {
                    ID = 987654321,
                    customerID = "987654321",
                    type = "test2",
                    description = "test description 2",
                    onDate = new DateTime(2021, 1, 20),
                    read = false,
                    saved = false
                };

                controller = new NotificationsController(context);

                var result = await controller.Create(notification: tempNotif);
                Assert.IsType<RedirectToActionResult>(result);

                var viewRes = result as RedirectToActionResult;
                Assert.Equal("Index", viewRes.ActionName);
            }
        }

        [Fact]
        public async void TestDeleteConfirmed()
        {
            using (var context = new CommerceBankDbContext(TestDbContextOptions()))
            {
                Notification tempNotif = new Notification()
                {
                    ID = 987654321,
                    customerID = "987654321",
                    type = "test2",
                    description = "test description 2",
                    onDate = new DateTime(2021, 1, 20),
                    read = false,
                    saved = false
                };

                context.Add(tempNotif);

                Assert.Equal(tempNotif, context.Notification.Find(987654321));

                controller = new NotificationsController(context);

                var result = await controller.DeleteConfirmed(987654321);
                Assert.IsType<RedirectToActionResult>(result);

                Assert.Null(context.Notification.Find(987654321));

                var viewRes = result as RedirectToActionResult;
                Assert.Equal("Index", viewRes.ActionName);
            }
        }

        [Fact]
        public async void TestEditPOSTWithValidModelState()
        {
            using (var context = new CommerceBankDbContext(TestDbContextOptions()))
            {
                Notification tempNotif = new Notification()
                {
                    ID = 987654321,
                    customerID = "987654321",
                    type = "test2",
                    description = "test description 2",
                    onDate = new DateTime(2021, 1, 20),
                    read = false,
                    saved = false
                };

                context.Add(tempNotif);
                context.SaveChanges();

                controller = new NotificationsController(context);

                var result = await controller.Edit(987654321, tempNotif);
                Assert.IsType<RedirectToActionResult>(result);

                var viewRes = result as RedirectToActionResult;
                Assert.Equal("Index", viewRes.ActionName);
            }
        }

        [Fact]
        public async void TestEditPOSTWithInvalidID()
        {
            using (var context = new CommerceBankDbContext(TestDbContextOptions()))
            {
                Notification tempNotif = new Notification()
                {
                    ID = 987654321,
                    customerID = "987654321",
                    type = "test2",
                    description = "test description 2",
                    onDate = new DateTime(2021, 1, 20),
                    read = false,
                    saved = false
                };

                context.Add(tempNotif);
                context.SaveChanges();

                controller = new NotificationsController(context);

                var result = await controller.Edit(123456789, tempNotif);
                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}