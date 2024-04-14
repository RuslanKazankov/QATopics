using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu.Implications
{
    public class AnswersOnMyQuestionsMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            List<Answer> answers = PseudoDB.Answers.Where((a) => a.ResponderId == User.Id).TakeLast(20).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (Answer answer in answers)
            {
                sb.AppendLine("Вопрос #" + answer.QuestionId);
                sb.AppendLine("Вы: " + answer.Question.Text);
                sb.AppendLine("Ответ: " + answer.Text);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public override string GetNameOfMenu()
        {
            return nameof(AnswersOnMyQuestionsMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboard = new(new KeyboardButton[] {
                new KeyboardButton("Назад")
            });
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;
        }

        public override CommandResponse? SendCommand(string command)
        {
            return new CommandResponse(new MainMenu(this));
        }
    }
}
