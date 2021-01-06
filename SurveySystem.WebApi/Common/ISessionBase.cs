using System;

namespace SurveySystem.WebApi.Common
{
    public interface ISessionBase
    {
        Guid SessionId { get; }
    }
}
