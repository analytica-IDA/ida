using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace backend.Middleware
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLoggingMiddleware> _logger;
        private readonly string _logDirectory;

        public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var logPath = Path.Combine(_logDirectory, $"backend_error_{DateTime.Now:yyyy_MM_dd}.log");
            
            var errorDetails = new
            {
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Path = context.Request.Path,
                Method = context.Request.Method,
                Message = exception.Message,
                StackTrace = exception.StackTrace
            };

            var logLine = $"[{errorDetails.Timestamp}] HTTP {errorDetails.Method} {errorDetails.Path}\nErro: {errorDetails.Message}\nStack: {errorDetails.StackTrace}\n------------------------------------------------------------\n";
            
            try
            {
                await File.AppendAllTextAsync(logPath, logLine);
            }
            catch (Exception writeEx)
            {
                _logger.LogError(writeEx, "Falha crítica ao escrever log em arquivo físico.");
            }

            _logger.LogError(exception, "Um erro não tratado ocorreu e foi salvo em {LogPath}", logPath);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var result = JsonSerializer.Serialize(new { 
                message = "Erro interno no servidor. Ocorrência registrada no log.", 
                details = exception.Message 
            });

            await context.Response.WriteAsync(result);
        }
    }
}
