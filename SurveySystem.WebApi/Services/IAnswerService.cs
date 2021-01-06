using Microsoft.AspNetCore.Http;

namespace SurveySystem.WebApi.Services
{
    public interface IAnswerService
    {
        void AddAttachments(IFormFileCollection files);
    }
}
