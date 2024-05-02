using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public DbSet<AdminSettings> AdminSettings { get; set; } = null!;
        public DbSet<UserSettings> UserSettings { get; set; } = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies()
                .UseSqlServer(Config.LocalDbConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Questions)
                .WithOne(q => q.User)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            //UserSettings userSettings = new UserSettings() { Id = 1 };
            //modelBuilder.Entity<UserSettings>().HasData(userSettings);

            //User user = new User(Config.AdminChatId, nameof(MainMenu), userSettings.Id, "Администратор Тайлер");
            //modelBuilder.Entity<User>().HasData(user);

            //UserSettings userSettings1 = new UserSettings() { Id = 2 };
            //modelBuilder.Entity<UserSettings>().HasData(userSettings1);

            //User user1 = new User(501340703, nameof(MainMenu), userSettings1.Id, "Йана");
            //modelBuilder.Entity<User>().HasData(user1);

            //AdminSettings adminSettings = new AdminSettings() { Id = 1 };
            //modelBuilder.Entity<AdminSettings>().HasData(adminSettings);

            //Admin admin = new Admin(user.Id, adminSettings.Id) { Id = 1 };
            //modelBuilder.Entity<Admin>().HasData(admin);
        }
    }
}
