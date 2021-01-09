using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SurveySystem.WebApi.Common;
using SurveySystem.WebApi.Context;
using SurveySystem.WebApi.DbModels;
using SurveySystem.WebApi.Request;

namespace SurveySystem.WebApi.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly SurveySystemDbContext _dbContext;

        private readonly ILogger _logger;
        private readonly Guid _sessionId;

        public AnswerService(ILoggerFactory loggerFactory, ISurveySystemDbContext dbContext, ISessionBase sessionBase)
        {
            _dbContext = (SurveySystemDbContext)dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            
            _dbContext.Database.EnsureCreated();
            
            _logger = loggerFactory.CreateLogger<AnswerService>();
            _sessionId = sessionBase.SessionId;
        }

        public async Task AddAttachmentsAsync(Guid answerId, IFormFileCollection files)
        {
            _logger.LogDebug($"Enter in method '{nameof(AddAttachmentsAsync)}.[{_sessionId}]'");
            if (!files?.Any() ?? true)
            {
                _logger.LogError($"Список вложений пуст.[{_sessionId}]");
                throw new ArgumentException("Передан пустой список файлов");
            }

            foreach (var file in files)
            {
                _dbContext.AnswerAttachments.Add(new AnswerAttachmentModel()
                {
                    AnswerId = answerId,
                    FileName = file.FileName,
                    Id = Guid.NewGuid(),
                    Size = file.Length,
                    Created = DateTime.UtcNow,
                    MimeType = file.ContentType
                });
            }

            await _dbContext.SaveChangesAsync();
            
            _logger.LogDebug($"Exit from method '{nameof(AddAttachmentsAsync)}.[{_sessionId}]'");

        }


        public async Task AddAnswerEventsAsync(Guid answerId, AnswerEvent[] answerEvents)
        {
            _logger.LogDebug($"Enter in method '{nameof(AddAnswerEventsAsync)}.[{_sessionId}]'");
            if (!answerEvents?.Any() ?? true)
            {
                _logger.LogError($"Список действий пользователя пуст![{_sessionId}]'"); // Хотя возможно и требуется. Это не уточнено в ТЗ
                throw new ArgumentException("Пустая коллекция эвентов", nameof(answerEvents));
            }

            foreach (var answerEvent in answerEvents)
            {
                _dbContext.AnswerEvents.Add(new AnswerEventModel
                {
                    AnswerId = answerId,
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    ClientTime = answerEvent.ClientTime,
                    Type = (int)answerEvent.Type,
                    Value = answerEvent.Value
                });
            }
            
            await _dbContext.SaveChangesAsync();
            
            _logger.LogDebug($"Exit from method '{nameof(AddAnswerEventsAsync)}.[{_sessionId}]'");

        }
        
        
    }
}
