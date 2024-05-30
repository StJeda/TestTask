using Microsoft.AspNetCore.Mvc;
using Moq;
using TestTask.API.Controllers;
using TestTask.Core;
using TestTask.Core.Services.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Test
{

    [TestFixture]
    public class EFControllerTests
    {
        [SetUp]
        public void Setup()
        {
            var mockService = new Mock<IEFDocumentService>();
            mockService = new Mock<IEFDocumentService>();
            var controller = new EFController(mockService.Object);
            controller = new EFController(mockService.Object);
        }

        [Test]
        public async Task EFController_Get_ReturnsOkWithDocuments()
        {
            var mockService = new Mock<IEFDocumentService>();
            var documents = new List<Document> { new Document { DocumentId = 1, Description = "Test", Amount = 10 } };
            mockService.Setup(service => service.Get()).ReturnsAsync(documents);
            var controller = new EFController(mockService.Object);

            var result = await controller.Get();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<IEnumerable<Document>>(okResult.Value);
            Assert.AreEqual(documents, okResult.Value);
        }
        [Test]
        public async Task Get_ReturnsListOfDocuments()
        {
            // Arrange
            var mockService = new Mock<IEFDocumentService>();
            var expectedDocuments = new List<Document>() { new Document(), new Document() };
            mockService.Setup(service => service.Get()).ReturnsAsync(expectedDocuments);
            var controller = new EFController(mockService.Object);
            // Act
            var result = await controller.Get() as OkObjectResult;
            var documents = result?.Value as List<Document>;

            // Assert
            Assert.IsNotNull(documents);
            Assert.AreEqual(expectedDocuments.Count, documents.Count);
        }
      
    }
}
