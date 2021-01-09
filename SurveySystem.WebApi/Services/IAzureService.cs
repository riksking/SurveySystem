using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SurveySystem.WebApi.Services
{
    public interface IAzureService
    {
        Task AddFilesToBlob(IFormFileCollection files);
    }
}
