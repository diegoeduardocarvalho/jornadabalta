using Fina.Api.Data;
using Fina.Api.Handlers;
using Fina.Core.Handlers;
using Fina.Core.Requests.Categories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string connectionString = "server=localhost;user=root;password=r00t04kt3ch;database=finadb";
var serverVersion = new MySqlServerVersion(new Version(8, 0, 2));

builder.Services.AddDbContext<AppDbContext>(
    dbContextOptions => dbContextOptions
            .UseMySql(connectionString, serverVersion)
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
);

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

var app = builder.Build();

app.MapGet("/", (GetCategoryByIdRequest request, ICategoryHandler handler)
    => handler.GetByIdAsync(request));

app.Run();