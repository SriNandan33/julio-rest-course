using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.ErrorHandling;

public static class ErrorHandlingExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
            .CreateLogger("Error Handling");

            var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();

            var exception = exceptionDetails?.Error;

            var traceId = Activity.Current?.TraceId;
            logger.LogError(exception, "Cloud not process a request on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, traceId);

            var problem = new ProblemDetails
            {
                Title = "We made a mistake but we are working on it!",
                Status = StatusCodes.Status500InternalServerError,
                Extensions = {
                    {"TraceID", traceId.ToString()}
                }
            };

            var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
            if (environment.IsDevelopment())
            {
                problem.Detail = exception?.ToString();
            }

            await Results.Problem(problem).ExecuteAsync(context);
        });
    }
}