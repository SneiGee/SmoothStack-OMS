using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Order.Data;
using Order.Persistence;
using Order.Shared.Behaviors;
using Order.Shared.Exceptions;

namespace Order.Extensions;

public static class BuilderExtensions
{
    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(n => n.FullName); });
    }

    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(o =>
                o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    }

    public static void AddMediatr(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblies(typeof(Program).Assembly);
            c.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
            c.AddOpenBehavior(typeof(LoggingBehavior<,>));
            c.AddOpenBehavior(typeof(ValidationBehavior<,>));

        });
    }

    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    }

    public static void AddCarter(this WebApplicationBuilder builder)
        => builder.Services.AddCarter();

    public static void AddValidators(this WebApplicationBuilder builder)
        => builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    public static void AddExceptionHandler(this WebApplicationBuilder builder)
        => builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    public static void AddHealthChecks(this WebApplicationBuilder builder)
        => builder.Services.AddHealthChecks()
            .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "PostgreSQL");

}
