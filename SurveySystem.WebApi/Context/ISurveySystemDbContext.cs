using Microsoft.EntityFrameworkCore;
using SurveySystem.WebApi.DbModels;

namespace SurveySystem.WebApi.Context
{
    public interface ISurveySystemDbContext
    {
        DbSet<AnswerAttachmentModel> AnswerAttachments { get; set; }
        DbSet<AnswerEventModel> AnswerEvents { get; set; }
    }
}
