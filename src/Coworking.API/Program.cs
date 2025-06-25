using Coworking.Aplication;
using Coworking.Aplication.Exceptions;
using Coworking.Domain.Configuration;
using Coworking.Domain.Interfaces;
using Coworking.Domain.Repositories;
using Coworking.Infra;
using Coworking.Infra.Repositories;
using Coworking.Infra.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(ApplicationLayer).Assembly,
        typeof(Program).Assembly
    );
});

var connectionString = builder.Configuration.GetConnectionString("CoworkingConnection");
builder.Services.AddDbContext<DefaultContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

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

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = errorFeature.Error;

        var problemDetails = new ProblemDetails
        {
            Title = "Ocorreu um erro",
            Status = exception switch
            {
                BusinessException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            },
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = problemDetails.Status.Value;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

app.MapGet("/health", () =>
{
    return TypedResults.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
}).WithTags("Diagnostics").AllowAnonymous();

app.Run();
