using QATopics.Helpers;
using QATopics.Models.Database;

namespace QATopics.Services.Implications
{
    public class RoleService
    {
        public static bool IsAdmin(long userId)
        {
            return PseudoDB.Admins.Where((a)=>a.UserId == userId).FirstOrDefault() != null;
        }

        public static void DoAdmin(long userId)
        {
            if (!IsAdmin(userId))
                PseudoDB.Admins.Add(new Admin() { UserId = userId });
        }
    }
}
