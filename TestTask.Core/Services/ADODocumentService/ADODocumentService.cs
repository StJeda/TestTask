

using TestTask.Core.Services.Interfaces;
using TestTask.DAL.Interfaces;
using TestTask.Domain.Models;

namespace TestTask.Core.Services.ADODocumentService
{
    public class ADODocumentService(IADODocumentRepository repository) : IADODocumentService
    {
        private readonly IADODocumentRepository _repository = repository;

        public async Task<Document?> Get(int Id)
        {
            var document = await _repository.Get(Id);
            return document;
        }

        public async Task<IEnumerable<Document>> Get()
        {
            var documents = await _repository.Get();
            return documents;
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
            catch (ArgumentNullException ex)
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
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}