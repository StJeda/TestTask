using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestTask.Core.Services.Interfaces;
using TestTask.DAL.Interfaces;
using TestTask.DAL.Repository;
using TestTask.Domain.Models;

namespace TestTask.Core
{
    public class EFDocumentService(IEFDocumentRepository repository) : IEFDocumentService
    {
        private readonly IEFDocumentRepository _repository = repository;
        

        public async Task<Document> Get(int Id)
        {
            var document = await _repository.Get(Id);
            return document;
        }

        public async Task<IEnumerable<Document>> Get()
        {
            var document = await _repository.Get();
            return document;
        }

        public async Task<bool> Post(Document document)
        {
            try
            {
                if (document is null)
                    throw new ArgumentNullException(nameof(document));
                await _repository.Post(document);
                return true;
            }
            catch(ArgumentNullException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public async Task<bool> Delete(int Id)
        {
            try
            {
                await _repository.Delete(Id);
                return true;
            }
            catch (ArgumentException ex) {
                Console.WriteLine(ex.ToString());
                return false;
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}
