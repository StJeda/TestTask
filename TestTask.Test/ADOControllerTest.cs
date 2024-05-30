using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.API.Controllers;
using TestTask.Core.Services.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Test
{
    [TestFixture]
    public class ADOControllerTest
    {
        [SetUp]
        public void Setup()
        {
            var mockService = new Mock<IADODocumentService>();
            mockService = new Mock<IADODocumentService>();
            var controller = new ADOController(mockService.Object);
            controller = new ADOController(mockService.Object);
        }
        [Test]
        public async Task ADOController_Delete_ReturnsOk()
        {
            // Arrange
            var mockService = new Mock<IADODocumentService>();
            mockService.Setup(service => service.Delete(It.IsAny<int>())).ReturnsAsync(true);
            var controller = new ADOController(mockService.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task ADOController_Get_ReturnsOkWithDocuments()
        {
            // Arrange
            var mockService = new Mock<IADODocumentService>();
            var documents = new List<Document> { new Document { DocumentId = 1, Description = "Test", Amount = 10 } };
            mockService.Setup(service => service.Get()).ReturnsAsync(documents);
            var controller = new ADOController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<IEnumerable<Document>>(okResult.Value);
            Assert.AreEqual(documents, okResult.Value);
        }
        [Test]
        public async Task Get_ReturnsNotFound_WhenNoDocumentsExist()
        {
            // Arrange
            var mockService = new Mock<IADODocumentService>();
            mockService.Setup(service => service.Get()).ReturnsAsync((List<Document>)null);
            var controller = new ADOController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_ReturnsBadRequest_WhenIdIsInvalid()
        {
            // Arrange
            int invalidId = -1;
            var mockService = new Mock<IADODocumentService>();
            var controller = new ADOController(mockService.Object);

            // Act
            var result = await controller.Get(invalidId);

            // Assert
            Assert.IsInstanceOf<ActionResult<Document>>(result);
        }

    }
}
