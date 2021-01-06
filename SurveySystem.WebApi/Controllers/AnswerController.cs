using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SurveySystem.WebApi.Common;
using SurveySystem.WebApi.Request;
using SurveySystem.WebApi.Services;

namespace SurveySystem.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAnswerService _answerService;
        private readonly Guid _sessionId;

        public AnswerController(ILoggerFactory logger, IAnswerService answerService, ISessionBase sessionBase)
        {
            _logger = logger.CreateLogger<AnswerController>();
            _answerService = answerService;
            _sessionId = sessionBase.SessionId;
        }

        /// <summary>
        /// Метод сохраняет вложения ответа
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Request is created</response>
        /// <response code="400">Invalid request</response>
        /// <response code="409">Conflict (Duplicate records)</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [HttpPost("answers/{answerId}/attachments")]
        public async Task<ActionResult> SaveAttachments([Required][FromRoute] Guid answerId, IFormFileCollection files)
        {
            _logger.LogInformation($"Поступил запрос на сохранение вложений для ответа '{answerId}'.[{_sessionId}]");
            
            /*var files = Request.Form.Files;*/
            if (!files?.Any() ?? true)
            {
                _logger.LogDebug($"Список вложений пуст. Сохранение данных не требуется.[{_sessionId}]");
                return Ok();
            }

            await _answerService.AddAttachmentsAsync(answerId, files);
            
            return await Task.FromResult(Ok());
        }

        /// <summary>
        /// Метод сохраняет действие пользователя при ответе
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Request is created</response>
        /// <response code="400">Invalid request</response>
        /// <response code="409">Conflict (Duplicate records)</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        [HttpPost("answers/{answerId}/events")]
        public async Task<ActionResult> SaveAnswerEvents([Required][FromRoute] Guid answerId, [FromBody] AnswerEvent[] answerEvents)
        {
            _logger.LogInformation($"Поступил запрос на сохранение действий пользователя при ответе на вопрос '{answerId}'.[{_sessionId}]");

            if (!answerEvents?.Any() ?? true)
            {
                _logger.LogDebug("Список действий пользователя пуст. Сохранение данных не требуется!"); // Хотя возможно и требуется. Это не уточнено в ТЗ
                return Ok();
            }

            await _answerService.AddAnswerEventsAsync(answerId, answerEvents);
            
            return await Task.FromResult(Ok());

        }
    }
}
