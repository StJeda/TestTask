
using Microsoft.Data.SqlClient;
using TestTask.DAL.Interfaces;
using TestTask.Domain.Models;
using TestTask.Domain.Models.Enums;

namespace TestTask.DAL.Repository
{
    public class ADODocumentRepository : IADODocumentRepository
    {
        private readonly string _connectionString;

        public ADODocumentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("DELETE FROM Documents WHERE DocumentId = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Document?> Get(int id)
        {
            Document document = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM Documents WHERE DocumentId = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var statuses = await GetDocumentStatuses(id);

                            document = new Document
                            {
                                DocumentId = Convert.ToInt32(reader["DocumentId"]),
                                Amount = Convert.ToInt32(reader["Amount"]),
                                Description = (string)reader["Description"],
                                Statuses = statuses.ToList()
                            };
                        }
                    }
                }
            }

            return document;
        }

        public async Task<IEnumerable<Document>> Get()
        {
            var documents = new List<Document>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM Documents", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var id = Convert.ToInt32(reader["DocumentId"]);
                        var statuses = await GetDocumentStatuses(id);

                        var document = new Document
                        {
                            DocumentId = id,
                            Amount = Convert.ToInt32(reader["Amount"]),
                            Description = (string)reader["Description"],
                            Statuses = statuses.ToList()
                        };

                        documents.Add(document);
                    }
                }
            }

            return documents;
        }

        public async Task Post(Document document)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SqlCommand("INSERT INTO Documents (Description, Amount) VALUES (@Description, @Amount); SELECT SCOPE_IDENTITY();", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@Description", document.Description);
                            command.Parameters.AddWithValue("@Amount", document.Amount);
                            var newDocumentId = Convert.ToInt32(await command.ExecuteScalarAsync());
                            using (var statusCommand = new SqlCommand("INSERT INTO DocumentStatuses (DocumentId, StatusId, DateTime) VALUES (@DocumentId, @StatusId, @DateTime)", connection, transaction))
                            {
                                statusCommand.Parameters.AddWithValue("@DocumentId", newDocumentId);
                                statusCommand.Parameters.AddWithValue("@StatusId", (int)DocumentStatusEnum.CREATED);
                                statusCommand.Parameters.AddWithValue("@DateTime", DateTime.Now);
                                await statusCommand.ExecuteNonQueryAsync();
                            }
                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task Save(){}

        public void Dispose() {}

        private async Task<IEnumerable<DocumentStatus>> GetDocumentStatuses(int documentId)
        {
            var statuses = new List<DocumentStatus>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM DocumentStatuses WHERE DocumentId = @Id ORDER BY DateTime DESC", connection))
                {
                    command.Parameters.AddWithValue("@Id", documentId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var status = new DocumentStatus
                            {
                                DocumentStatusId = Convert.ToInt32(reader["DocumentStatusId"]),
                                DocumentId = Convert.ToInt32(reader["DocumentId"]),
                                StatusId = Convert.ToInt32(reader["StatusId"]),
                                DateTime = Convert.ToDateTime(reader["DateTime"])
                            };

                            if (status.StatusId != (int)DocumentStatusEnum.DELETED)
                                statuses.Add(status);
                        }
                    }
                }
            }

            return statuses;
        }
    }
}
