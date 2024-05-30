using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Domain.Models;

namespace TestTask.DAL.Interfaces
{
    public interface IADODocumentRepository
    {
        Task<Document> Get(int id);
        Task<IEnumerable<Document>> Get();
        Task Delete(int id);
        Task Post(Document document);
        Task Save();
    }
}
