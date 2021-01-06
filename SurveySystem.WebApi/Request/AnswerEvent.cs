using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SurveySystem.WebApi.Request
{
    [Serializable]
    public class AnswerEvent
    {
        [Description("Значение")]
        public string Value { get; set;}
        
        [Description("Тип")]
        [Range(0,4)]
        public AnswerEventTypeEnum Type { get; set; }
        
        [Description("Дата события")]
        [DataType(DataType.DateTime)]
        public DateTime ClientTime { get; set; }
    }
}
