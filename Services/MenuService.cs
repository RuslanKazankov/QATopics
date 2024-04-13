using QATopics.Models.Database;
using QATopics.Models.Menu;

namespace QATopics.Services
{
    public class MenuService
    {
        public static BaseMenu GetMenuOfUser(User user)
        {
            return user.CurrentMenu switch
            {
                nameof(MainMenu) => new MainMenu(user),
                nameof(AskQuestion) => new AskQuestion(user),
                nameof(ChangeNameMenu) => new ChangeNameMenu(user),
                nameof(MyQuestionsMenu) => new MyQuestionsMenu(user),
                nameof(QuestionsMenu) => new QuestionsMenu(user),
                nameof(AnswerTheQuestionMenu) => new AnswerTheQuestionMenu(user),
                _ => new MainMenu(user),
            };
        }
    }
}
