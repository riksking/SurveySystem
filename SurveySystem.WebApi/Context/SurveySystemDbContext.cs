using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SurveySystem.WebApi.DbModels;

namespace SurveySystem.WebApi.Context
{
    public class SurveySystemDbContext : DbContext, ISurveySystemDbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public SurveySystemDbContext(DbContextOptions options, ILoggerFactory loggerFactory) 
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseLoggerFactory(_loggerFactory);

        
        public virtual DbSet<AnswerAttachmentModel> AnswerAttachments { get; set; }
        public virtual DbSet<AnswerEventModel> AnswerEvents { get; set; }
    }
}
