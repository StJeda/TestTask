using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using TestTask.Core.Services.Interfaces;
using TestTask.Domain.Models;
using Document = TestTask.Domain.Models.Document;

namespace TestTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EFController(IEFDocumentService service) : ControllerBase
    {
        private readonly IEFDocumentService _service = service;
        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
            var result = await _service.Delete(id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Get() {
            var result = await _service.Get();
            return Ok(result);
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Document?>> Get(int Id)
        {
            var result = await _service.Get(Id);
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> Post(string description, int amount)
        {
            var document = new Document
            {
                Description = description,
                Amount = amount
            };
            var result = await _service.Post(document);
            return Ok(result);
        }
    }
}
