using Microsoft.EntityFrameworkCore;
using SurveySystem.WebApi.DbModels;

namespace SurveySystem.WebApi.Context
{
    public class SurveySystemDbContext : DbContext, ISurveySystemDbContext
    {
        protected SurveySystemDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public virtual DbSet<AnswerAttachment> AnswerAttachments { get; set; }
 
    }
}
