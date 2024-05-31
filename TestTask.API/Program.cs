using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestTask.Core;
using TestTask.Core.Services.ADODocumentService;
using TestTask.Core.Services.Interfaces;
using TestTask.DAL.Context;
using TestTask.DAL.Interfaces;
using TestTask.DAL.Repository;
namespace TestTask.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DocumentContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("TestTask.API")));
            builder.Services.AddScoped<IEFDocumentRepository, EFDocumentRepository>();
            builder.Services.AddScoped<IEFDocumentService, EFDocumentService>();

            builder.Services.AddScoped<IADODocumentRepository, ADODocumentRepository>(provider =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new ADODocumentRepository(connectionString);
            });
            builder.Services.AddScoped<IADODocumentService, ADODocumentService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
