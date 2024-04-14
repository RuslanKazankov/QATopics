using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users = null!;
        public DbSet<Question> Questions = null!;
        public DbSet<Answer> Answers = null!;
        public DbSet<QuestionReport> QuestionReports = null!;
        public DbSet<AnswerReport> AnswerReports = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=qatopicsdb;Trusted_Connection=True;");
        }
    }
}
