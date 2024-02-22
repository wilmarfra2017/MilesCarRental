using MilesCarRental.Domain.Exceptions;
using System.Net;

namespace MilesCarRental.Api.Middleware
{
    /// <summary>
    /// Define un middleware personalizado para manejar excepciones globalmente en la aplicación.    
    /// </summary>      
    public class AppExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AppExceptionHandlerMiddleware> _logger;
        
        public AppExceptionHandlerMiddleware(RequestDelegate next, ILogger<AppExceptionHandlerMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next)); // 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        }

        // Método invocado por el runtime de ASP.NET Core para procesar las solicitudes HTTP.
        public async Task InvokeAsync(HttpContext context)
        {
            EnsureValidContext(context);
            await HandleInvocationAsync(context);
        }

        // Verifica que el contexto HTTP proporcionado no sea nulo.
        private static void EnsureValidContext(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
        }

        // Intenta ejecutar el próximo middleware en la pipeline y maneja cualquier excepción que ocurra.
        private async Task HandleInvocationAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (CoreBusinessException ex)
            {
                HandleException(ex, context, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                HandleException(ex, context, HttpStatusCode.InternalServerError);
            }
        }

        // Realiza el manejo de la excepción, registrando el error y preparando la respuesta HTTP.
        private void HandleException(Exception ex, HttpContext context, HttpStatusCode statusCode)
        {
            string loggingMessageTemplate = "An error occurred: {Error}";
            _logger.LogError(loggingMessageTemplate, ex.Message);

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                ErrorMessage = ex.Message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            context.Response.WriteAsync(result);
        }
    }
}
