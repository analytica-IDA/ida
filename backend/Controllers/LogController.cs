using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // Permite erros mesmo deslogado (ex: auth failed do front)
    public class LogController : ControllerBase
    {
        private readonly string _logDirectory;

        public LogController()
        {
            // O frontend roda em C:/Portifolio/analytica/ida/frontend
            // O backend roda em C:/Portifolio/analytica/ida/backend
            var rootDir = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName ?? "";
            _logDirectory = Path.Combine(rootDir, "frontend", "logs");

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        [HttpPost("frontend")]
        public async Task<IActionResult> LogFrontendError([FromBody] FrontendErrorRequest request)
        {
            var logPath = Path.Combine(_logDirectory, $"frontend_error_{DateTime.Now:yyyy_MM_dd}.log");

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var userDetail = request.User ?? "Desconhecido/Anônimo";
            
            var logLine = $"[{timestamp}] Usuario: {userDetail}\nErro gerado em: {request.Url}\n{request.Message}\nStack: {request.Stack}\nData Adicional: {request.AdditionalData}\n------------------------------------------------------------\n";
            
            try
            {
                await System.IO.File.AppendAllTextAsync(logPath, logLine);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao gravar log no disco.", details = ex.Message });
            }
        }
    }

    public class FrontendErrorRequest
    {
        public string? Message { get; set; }
        public string? Stack { get; set; }
        public string? Url { get; set; }
        public string? AdditionalData { get; set; }
        public string? User { get; set; }
    }
}
