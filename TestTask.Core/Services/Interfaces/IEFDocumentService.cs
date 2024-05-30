using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Domain.Models;

namespace TestTask.Core.Services.Interfaces
{
    public interface IEFDocumentService
    { 
       public Task<Document> Get(int Id);
        public Task<IEnumerable<Document>> Get();

        public Task<bool> Delete(int Id);
        public Task<bool> Post(Document document);
    }
}
