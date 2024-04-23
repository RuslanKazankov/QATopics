using Microsoft.EntityFrameworkCore;
using QATopics.Helpers;
using QATopics.Models.Menu.Implications;

namespace QATopics.Models.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<QuestionReport> QuestionReports { get; set; } = null!;
        public DbSet<AnswerReport> AnswerReports { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=qatopicsdb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Questions)
                .WithOne(q => q.User)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.CurrentQuestion)
                .WithOne()
                .HasForeignKey<User>(u => u.CurrentQuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.CurrentAnswer)
                .WithOne()
                .HasForeignKey<User>(u => u.CurrentAnswerId)
                .OnDelete(DeleteBehavior.NoAction);

            User user = new User(Config.AdminChatId, nameof(MainMenu), "Администратор Тайлер");
            modelBuilder.Entity<User>().HasData(user);

            AdminSettings adminSettings = new AdminSettings() { Id = 1};
            modelBuilder.Entity<AdminSettings>().HasData(adminSettings);

            Admin admin = new Admin(user.Id, adminSettings.Id) { Id = 1 };
            modelBuilder.Entity<Admin>().HasData(admin);
        }
    }
}
