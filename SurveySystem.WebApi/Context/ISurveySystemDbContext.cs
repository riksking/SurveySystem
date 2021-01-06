using Microsoft.EntityFrameworkCore;
using SurveySystem.WebApi.DbModels;

namespace SurveySystem.WebApi.Context
{
    public interface ISurveySystemDbContext
    {
        DbSet<AnswerAttachment> AnswerAttachments { get; set; }
    }
}
