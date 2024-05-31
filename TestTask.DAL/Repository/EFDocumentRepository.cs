using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestTask.DAL.Context;
using TestTask.DAL.Interfaces;
using TestTask.Domain.Models;
using TestTask.Domain.Models.Enums;

namespace TestTask.DAL.Repository
{
    public class EFDocumentRepository(DocumentContext context) : IEFDocumentRepository
    {
        private readonly DocumentContext _context = context;

        public async Task Delete(int Id)
        {
            var document = await _context.Documents.AsQueryable()
                .AsTracking().FirstOrDefaultAsync(d => d.DocumentId.Equals(Id));

            if (document != null)
            {
                await _context.Entry(document)
                    .Collection(d => d.Statuses)
                    .Query()
                    .OrderByDescending(s => s.DateTime)
                    .Take(1)
                    .AsQueryable()
                    .LoadAsync();

                var latestStatus = document.Statuses.FirstOrDefault();

                if (latestStatus is not null && latestStatus.StatusId is (int)DocumentStatusEnum.DELETED)
                {
                    throw new InvalidOperationException("Document is already deleted.");
                }
                else
                {
                    var newStatus = new DocumentStatus
                    {
                        DateTime = DateTime.Now,
                        DocumentId = document.DocumentId,
                        StatusId = (int)DocumentStatusEnum.DELETED,
                    };
                    document.Statuses.Add(newStatus);
                    _context.Documents.Update(document);
                    await Save();
                }
            }
            else
            {
                throw new ArgumentException($"Document is not found");
            }
        }


        public async Task<Document?> Get(int Id)
        {
            var document = await _context.Documents.AsQueryable().AsTracking().FirstOrDefaultAsync(d => d.DocumentId == Id);
            if(document is not null) 
                await _context.Entry(document)
                    .Collection(d => d.Statuses)
                    .Query()
                    .OrderByDescending(s => s.DateTime)
                    .Take(1)
                    .AsQueryable()
                    .LoadAsync();

            return document is not null && document.Statuses.LastOrDefault()?.StatusId != (int)DocumentStatusEnum.DELETED ? document : null;
        }

        public async Task<IEnumerable<Document>> Get()
        {
            var documents = await _context.Documents.AsQueryable().AsTracking().ToListAsync();

            if (documents is not null)
                foreach (var document in documents)
            {
                await _context.Entry(document)
                    .Collection(d => d.Statuses)
                    .Query()
                    .OrderByDescending(s => s.DateTime)
                    .Take(1)
                    .AsQueryable()
                    .LoadAsync();
            }

            return documents.Where(d => d.Statuses.LastOrDefault()?.StatusId != (int)DocumentStatusEnum.DELETED);
        }

        public async Task Post(Document document)
        {
            document.Statuses = new List<DocumentStatus>
                {
                    new DocumentStatus() {
                    DateTime = DateTime.Now,
                    StatusId = (int)DocumentStatusEnum.CREATED,
                    DocumentId = document.DocumentId,
                    }
                };
            await _context.AddAsync(document);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

