using System;

namespace SurveySystem.WebApi.Common
{
    class SessionBase : ISessionBase
    {
        private Guid? _sessionId;

        public Guid SessionId
        {
            get
            {
                _sessionId ??= Guid.NewGuid();
                return _sessionId.Value;
            }
        }
    }
}
