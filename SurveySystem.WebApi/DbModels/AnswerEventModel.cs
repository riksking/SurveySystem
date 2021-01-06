using System;

namespace SurveySystem.WebApi.DbModels
{
    public class AnswerEventModel
    {
        public Guid Id {get; set;}
        public Guid AnswerId { get; set; }
        public DateTime Created { get; set; }
        public string Value { get; set;}
        public int Type { get; set; }
        public DateTime ClientTime { get; set; }
        
    }
}
