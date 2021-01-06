using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SurveySystem.WebApi.Services
{
    public interface IAnswerService
    {
        Task AddAttachmentsAsync(Guid answerId, IFormFileCollection files);
    }
}
