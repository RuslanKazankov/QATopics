using QATopics.Models.Database;
using QATopics.Models.Menu;
using QATopics.Models.Menu.Implications;

namespace QATopics.Services.Implications
{
    public class MenuService
    {
        public static BaseMenu GetMenuOfUser(User user, IMessageService? messageService)
        {
            IMenuParams menuParams = new MenuParams(user, messageService);
            return GetMenuOfUser(menuParams);
        }
        public static BaseMenu GetMenuOfUser(IMenuParams menuParams)
        {
            return menuParams.User.CurrentMenu switch
            {
                nameof(AdminMenu) => new AdminMenu(menuParams),
                nameof(AnswerReportMenu) => new AnswerReportMenu(menuParams),
                nameof(AnswerReportsMenu) => new AnswerReportsMenu(menuParams),
                nameof(AnswerTheQuestionMenu) => new AnswerTheQuestionMenu(menuParams),
                nameof(AnswersOnMyQuestionsMenu) => new AnswersOnMyQuestionsMenu(menuParams),
                nameof(AskQuestionMenu) => new AskQuestionMenu(menuParams),
                nameof(ChangeNameMenu) => new ChangeNameMenu(menuParams),
                nameof(MainMenu) => new MainMenu(menuParams),
                nameof(MyQuestionsMenu) => new MyQuestionsMenu(menuParams),
                nameof(PopularQuestionsMenu) => new PopularQuestionsMenu(menuParams),
                nameof(QuestionReportMenu) => new QuestionReportMenu(menuParams),
                nameof(QuestionReportsMenu) => new QuestionReportsMenu(menuParams),
                nameof(QuestionsMenu) => new QuestionsMenu(menuParams),
                _ => new MainMenu(menuParams),
            };
        }
    }
}
