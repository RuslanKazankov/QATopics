using QATopics.Helpers;
using QATopics.Models.Database;

namespace QATopics.Services.Implications
{
    public class RoleService
    {
        public static bool IsAdmin(long userId)
        {
            using ApplicationContext Db = new ApplicationContext();
            return Db.Admins.Where(a => a.UserId == userId).FirstOrDefault() != null;
        }
    }
}
