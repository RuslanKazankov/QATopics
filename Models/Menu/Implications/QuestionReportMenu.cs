using QATopics.Helpers;
using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu.Implications
{
    public class QuestionReportMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            if (User.CurrentQuestion != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Вопрос #").AppendLine(User.CurrentQuestion.Id.ToString())
                    .Append("Вопрос: ").AppendLine(User.CurrentQuestion.Text);
                sb.Append("Укажите причину жалобы:");
                return sb.ToString();
            }
            return "Неизвестная ошибка";
        }

        public override string GetNameOfMenu()
        {
            return nameof(QuestionReportMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            return new KeyboardBuilder("Назад").BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "Назад")
            {
                return new CommandResponse(new QuestionsMenu(this));
            }
            if (User.CurrentQuestion != null)
            {
                QuestionReport questionReport = new QuestionReport(User.CurrentQuestion.Id, command);
                Db.QuestionReports.Add(questionReport);
                User.CurrentQuestion = null;
                return new CommandResponse(new QuestionsMenu(this)) { ResultMessage = "Жалоба отправлена" };
            }
            return new CommandResponse(new QuestionsMenu(this)) { ResultMessage = "Вопрос не найден"};
        }
    }
}
