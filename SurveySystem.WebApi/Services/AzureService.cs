using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SurveySystem.WebApi.Common;

namespace SurveySystem.WebApi.Services
{
    public class AzureService : IAzureService
    {
        private readonly ILogger _logger;
        private readonly Guid _sessionId;
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly string _answerFileContainerName = "AnswerFileContainer".ToLower();

        public AzureService(ILoggerFactory loggerFactory, IOptions<AzureServiceOptions> azureServiceOptions, ISessionBase sessionBase)
        {

            _cloudStorageAccount = string.IsNullOrWhiteSpace(azureServiceOptions?.Value?.ConnectionString) ? throw new ArgumentNullException(nameof(azureServiceOptions)) : CloudStorageAccount.Parse(azureServiceOptions.Value.ConnectionString);
            

            _logger = loggerFactory.CreateLogger<AnswerService>();
            _sessionId = sessionBase.SessionId;
        }

        public async Task AddFilesToBlob(IFormFileCollection files)
        {
            _logger.LogDebug($"Enter in method '{nameof(AddFilesToBlob)}'.[{_sessionId}]");
            
            if (!files?.Any() ?? true)
            {
                _logger.LogError($"Список вложений пуст.[{_sessionId}]");
                throw new ArgumentException("Передан пустой список файлов");
            }
            
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference(_answerFileContainerName);
            await container.CreateIfNotExistsAsync();

            foreach (var file in files)
            {
                _logger.LogDebug($"Upload file '{file.FileName}' to Azure. [{_sessionId}]");
                await using var stream = file.OpenReadStream();
                    
                var newBlob = container.GetBlockBlobReference(file.FileName);
                await newBlob.UploadFromStreamAsync(stream);
            }
            _logger.LogDebug($"Exit from method '{nameof(AddFilesToBlob)}'.[{_sessionId}]");
        }
    }
}
