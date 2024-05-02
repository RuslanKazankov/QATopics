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
            if (User.UserSettings!.CurrentQuestion != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Вопрос #").AppendLine(User.UserSettings!.CurrentQuestion.Id.ToString())
                    .Append("Вопрос: ").AppendLine(User.UserSettings!.CurrentQuestion.Text);
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
            if (User.UserSettings!.CurrentQuestion == null)
            {
                return new CommandResponse(new QuestionsMenu(this)) { ResultMessage = "Вопрос не найден" };
            }
            if (command.Length > Config.ReasonMessageLimit)
            {
                return new CommandResponse(this) { ResultMessage = $"Постарайтесь уместить всю боль в {Config.ReasonMessageLimit} символов" };
            }
            QuestionReport questionReport = new QuestionReport(User.UserSettings!.CurrentQuestion.Id, command);
            Db.QuestionReports.Add(questionReport);
            User.UserSettings!.CurrentQuestion = null;
            return new CommandResponse(new QuestionsMenu(this)) { ResultMessage = "Жалоба отправлена" };
        }
    }
}
