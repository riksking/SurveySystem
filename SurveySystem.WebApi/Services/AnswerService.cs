using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SurveySystem.WebApi.Common;
using SurveySystem.WebApi.Context;
using SurveySystem.WebApi.DbModels;

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
                _dbContext.AnswerAttachments.Add(new AnswerAttachment()
                {
                    AnswerId = answerId,
                    FileName = file.FileName,
                    Id = Guid.NewGuid(),
                    Size = file.Length,
                    Created = DateTime.UtcNow,
                    MimeType = file.ContentType
                });
            }

            await Task.CompletedTask;
            //Пока нет БД
            //await _dbContext.SaveChangesAsync();
        }
        
        
        
    }
}
