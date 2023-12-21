using Cdn.Freelance.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FreelanceContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("FreelanceDatabase"), npgsqlOption =>
        npgsqlOption.MigrationsHistoryTable(
            HistoryRepository.DefaultTableName,
            FreelanceContext.SchemaName
        )
    ).UseSnakeCaseNamingConvention();
});

var app = builder.Build();

app.Run();