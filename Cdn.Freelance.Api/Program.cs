using Cdn.Freelance.Api.Controllers;
using Cdn.Freelance.Api.Exceptions;
using Cdn.Freelance.Domain.SeedWork;
using Cdn.Freelance.Domain.Users;
using Cdn.Freelance.Infrastructure;
using Cdn.Freelance.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Okta.AspNetCore;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Security.Principal;

namespace Cdn.Freelance.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication(OktaDefaults.ApiAuthenticationScheme)
                .AddOktaWebApi(new OktaWebApiOptions()
                {
                    OktaDomain = builder.Configuration["Okta:OktaDomain"],
                    AuthorizationServerId = builder.Configuration["Okta:AuthorizationServerId"],
                    Audience = builder.Configuration["Okta:Audience"]
                });

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("cdn:freelance", policy =>
                {
                    policy.AuthenticationSchemes = new List<string> { OktaDefaults.ApiAuthenticationScheme };
                    policy.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", "cdn.freelance");
                });
            });

            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.ExampleFilters();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

            builder.Services.AddDbContext<FreelanceContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("FreelanceDatabase"), npgsqlOption =>
                    npgsqlOption.MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName,
                        FreelanceContext.SchemaName
                    )
                ).UseSnakeCaseNamingConvention();
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IPrincipal>(sp => sp.GetService<IHttpContextAccessor>()!.HttpContext!.User!);

            builder.Services.AddSingleton<IUserComposer, DefaultUserComposer>();
            builder.Services.AddScoped<IUserAccessor, UserAccessor>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            });

            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<UserAlreadyExistsExceptionHandler>();
            builder.Services.AddExceptionHandler<DefaultExceptionHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
