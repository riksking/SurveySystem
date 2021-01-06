using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SurveySystem.WebApi.Common;

namespace SurveySystem.WebApi.Services
{
    public class AnswerService : IAnswerService
    {

        private readonly ILogger _logger;
        private readonly Guid _sessionId;

        public AnswerService(ILoggerFactory loggerFactory, ISessionBase sessionBase)
        {
            _logger = loggerFactory.CreateLogger<AnswerService>();
            _sessionId = sessionBase.SessionId;
        }

        public void AddAttachments(IFormFileCollection files)
        {
            _logger.LogDebug($"Enter in method '{nameof(AddAttachments)}.[{_sessionId}]'");
            if (!files?.Any() ?? true)
            {
                _logger.LogError($"Список вложений пуст.[{_sessionId}]");
                throw new ArgumentException("Передан пустой список файлов");
            }
            
            
        }
        
        
        
    }
}
