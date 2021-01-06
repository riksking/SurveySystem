using System;

namespace SurveySystem.WebApi.DbModels
{
    public class AnswerAttachment
    {
        public Guid Id {get; set;}
        public Guid AnswerId { get; set; }
        public DateTime Created { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public int Size { get; set; }
    }
}
