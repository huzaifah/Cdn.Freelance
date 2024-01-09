using Cdn.Freelance.Api.Controllers;
using Cdn.Freelance.Api.Exceptions;
using Cdn.Freelance.Api.LayoutRenderers;
using Cdn.Freelance.Api.Middlewares;
using Cdn.Freelance.Domain.SeedWork;
using Cdn.Freelance.Domain.Users;
using Cdn.Freelance.Infrastructure;
using Cdn.Freelance.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using NLog;
using NLog.Web;
using Okta.AspNetCore;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Security.Principal;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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
            builder.Services.AddApiVersioning(setup =>
            {
                //indicating whether a default version is assumed when a client does
                // does not provide an API version.
                setup.AssumeDefaultVersionWhenUnspecified = true;

                setup.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

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

            builder.Services.AddHealthChecks()
                .AddDbContextCheck<FreelanceContext>("dbContextChecks", null, new List<string> { "Readiness" });

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
            builder.Services.AddExceptionHandler<ItemNotFoundExceptionHandler>();
            builder.Services.AddExceptionHandler<DefaultExceptionHandler>();

            builder.Host.UseNLog().ConfigureLogging((context, builder) =>
            {
                builder.ClearProviders();

                LogManager.Setup().SetupExtensions(s =>
                {
                    s.RegisterLayoutRenderer<CorrelationIdLayoutRenderer>("correlation-id");
                });

                var configuration =
                    new NLog.Extensions.Logging.NLogLoggingConfiguration(context.Configuration.GetSection("NLog"));
                builder.AddNLog(configuration);
            });

            var app = builder.Build();
            
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var descriptions = app.DescribeApiVersions();

                // Build a swagger endpoint for each discovered API version
                foreach (var description in descriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });

            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseExceptionHandler();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHealthChecks("/healthz/ready", new HealthCheckOptions()
            {
                Predicate = healthCheck => healthCheck.Tags.Contains("Readiness")
            }).RequireAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
