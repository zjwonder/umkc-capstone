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

namespace ProjectUnitTests
{
    public class TestTransactionsController
    {
        private readonly Mock<CommerceBankDbContext> mockDbContext = new Mock<CommerceBankDbContext>();
        TransactionsController controller;

        public TestTransactionsController()
        {
            CommerceBankProject.Models.AccountRecord tempRecord = new CommerceBankProject.Models.AccountRecord();
            tempRecord.actID = "123456789";
            tempRecord.actType = "Customer";
            mockDbContext.Object.Account.Add(tempRecord);

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

            mockDbContext.Object.Transaction.Add(tempTransaction);

            //mockDbContext.SetupAllProperties();

            //mockDbContext.Setup(x => x.Account=tempRecord);
            controller = new TransactionsController(mockDbContext.Object);
        }

        [Fact]
        public async void TestIndex()
        {
            var result = await controller.Index() as ViewResult;
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void TestFilterIndex()
        {

        }

        [Fact]
        public void TestGetDeleteView()
        {

            Assert.IsType<NotFoundResult>(controller.Delete(null).Result);
            Assert.IsType<ViewResult>(controller.Delete(123456789).Result);
        }
    }
}