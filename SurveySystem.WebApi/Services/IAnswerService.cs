using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SurveySystem.WebApi.Request;

namespace SurveySystem.WebApi.Services
{
    public interface IAnswerService
    {
        Task AddAttachmentsAsync(Guid answerId, IFormFileCollection files);
        Task AddAnswerEventsAsync(Guid answerId, AnswerEvent[] answerEvents);
    }
}
