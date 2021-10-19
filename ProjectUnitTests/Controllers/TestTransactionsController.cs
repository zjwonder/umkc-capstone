using System;
using Xunit;
using CommerceBankProject.Controllers;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using CommerceBankProject.Data;
using CommerceBankProject.Models;
using static ProjectUnitTests.Utilities.Utilities;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CommerceBankProject.Areas.Identity.Data;

namespace ProjectUnitTests
{
    public class TestTransactionsController
    {
        private readonly Mock<CommerceBankDbContext> mockDbContext = new Mock<CommerceBankDbContext>();
        private readonly Mock<TransactionsController> mockTransactionsController = new Mock<TransactionsController>();
        TransactionsController controller;

        public TestTransactionsController()
        {
            //AccountRecord tempRecord = new CommerceBankProject.Models.AccountRecord();
            //tempRecord.actID = "123456789";
            //tempRecord.actType = "Customer";
            //mockDbContext.Object.Account.Add(tempRecord);
            
            var tempTransaction = new Transaction
            {
                ID = 123456789,
                customerID = "123456789",
                actID = "123456789",
                actType = "Temp",
                onDate = new DateTime(2008, 1, 20),
                balance = 1234.56m,
                transType = "Fun",
                amount = 12.34m,
                description = "For fun",
                userEntered = true
            };

            //mockDbContext.Object.Transaction.Add(tempTransaction);

            //mockDbContext.SetupAllProperties();

            //mockDbContext.Setup(x => x.Account=tempRecord);
            //controller = new TransactionsController(mockDbContext.Object);
        }

        [Fact]
        public async void TestIndex()
        {
            List<Transaction> transactions = new List<Transaction>
            {
                new Transaction()
                {
                    ID = 789456123,
                    customerID = "123789456",
                    actID = "456123789",
                    actType = "Customer",
                    onDate = new DateTime(2008, 6, 1),
                    balance = 1000.04m,
                    transType = "Fun",
                    description = "test desc",
                    userEntered = false
                },
                new Transaction()
                {
                    ID = 456789123,
                    customerID = "111111111",
                    actID = "222222222",
                    actType = "Customer",
                    onDate = new DateTime(2021, 7, 2),
                    balance = 978.04m,
                    transType = "Gas",
                    description = "second description",
                    userEntered = false
                }
            };

            

            using (var context = new CommerceBankDbContext(TestDbContextOptions()))
            {
                foreach (Transaction t in transactions)
                {
                    context.Transaction.Add(t);
                }


                Mock<ApplicationUser> savedUser = new Mock<ApplicationUser>();
                //savedUser.Setup(x => x.Id).Returns(user.Identity.Name);

                savedUser.SetupAllProperties();
                //savedUser.Object.Id = "123456789";

                //savedUser.Object.Id = Guid.NewGuid().ToString();

                //var test = savedUser.Object.Id;

                context.Users.Add(savedUser.Object);

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {new Claim(ClaimTypes.NameIdentifier, savedUser.Object.Id),
            new Claim(ClaimTypes.Name, "test2")}, "TestAuthentication"));

                context.SaveChanges();
                controller = new TransactionsController(context);

                controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
                var result = await controller.Index();
                Assert.IsType<ViewResult>(result);
            }
            //var result = await controller.Index() as ViewResult;
            //Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void TestFilterIndex()
        {
            List<Transaction> transactions = new List<Transaction>
            {
                new Transaction()
                {
                    ID = 789456123,
                    customerID = "123789456",
                    actID = "456123789",
                    actType = "Customer",
                    onDate = new DateTime(2008, 6, 1),
                    balance = 1000.04m,
                    transType = "Fun",
                    description = "test desc",
                    userEntered = false
                },
                new Transaction()
                {
                    ID = 456789123,
                    customerID = "111111111",
                    actID = "222222222",
                    actType = "Customer",
                    onDate = new DateTime(2021, 7, 2),
                    balance = 978.04m,
                    transType = "Gas",
                    description = "second description",
                    userEntered = false
                }
            };



            using (var context = new CommerceBankDbContext(TestDbContextOptions()))
            {
                foreach (Transaction t in transactions)
                {
                    context.Transaction.Add(t);
                }


                Mock<ApplicationUser> savedUser = new Mock<ApplicationUser>();
                //savedUser.Setup(x => x.Id).Returns(user.Identity.Name);

                savedUser.SetupAllProperties();
                //savedUser.Object.Id = "123456789";

                //savedUser.Object.Id = Guid.NewGuid().ToString();

                //var test = savedUser.Object.Id;

                context.Users.Add(savedUser.Object);
                context.SaveChanges();

                var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {new Claim(ClaimTypes.NameIdentifier, savedUser.Object.Id),
            new Claim(ClaimTypes.Name, "test2")}, "TestAuthentication"));

                
                controller = new TransactionsController(context);

                controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
                var result = await controller.FilterIndex("all", "", "2006-1-1", "2021-1-1", "20");
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public void TestGetDeleteView()
        {

            Assert.IsType<NotFoundResult>(controller.Delete(null).Result);
            Assert.IsType<ViewResult>(controller.Delete(123456789).Result);
        }

        public void TestCreateTransaction()
        {

        }
    }
}