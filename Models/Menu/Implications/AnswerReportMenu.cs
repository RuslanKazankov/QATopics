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
    public class AnswerReportMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            if (User.UserSettings!.CurrentAnswer != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Ответ #").AppendLine(User.UserSettings!.CurrentAnswer.Id.ToString())
                    .Append("Ответ: ").AppendLine(User.UserSettings!.CurrentAnswer.Text);
                sb.Append("Укажите причину жалобы:");
                return sb.ToString();
            }
            return "Неизвестная ошибка";
        }

        public override string GetNameOfMenu()
        {
            return nameof(AnswerReportMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            return new KeyboardBuilder("Назад").BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (User.UserSettings!.CurrentAnswer == null)
            {
                return new CommandResponse(new AnswersOnMyQuestionsMenu(this)) { ResultMessage = "Ответ не найден" };
            }
            if (command == "Назад")
            {
                User.UserSettings!.CurrentAnswer = null;
                return new CommandResponse(new AnswersOnMyQuestionsMenu(this));
            }
            if (command.Length > Config.ReasonMessageLimit)
            {
                return new CommandResponse(this) { ResultMessage = $"Постарайтесь уместить всю боль в {Config.ReasonMessageLimit} символов" };
            }
            Db.AnswerReports.Add(new AnswerReport(User.UserSettings.CurrentAnswer.Id, command));
            Db.SaveChanges();
            User.UserSettings!.CurrentAnswer = null;
            return new CommandResponse(new AnswersOnMyQuestionsMenu(this)) { ResultMessage = "Жалоба отправлена" };
        }
    }
}
