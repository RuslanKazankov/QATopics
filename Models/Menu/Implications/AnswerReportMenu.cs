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
            if (User.CurrentAnswer != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Ответ #").AppendLine(User.CurrentAnswer.Id.ToString())
                    .Append("Ответ: ").AppendLine(User.CurrentAnswer.Text);
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
            if (User.CurrentAnswer == null)
            {
                return new CommandResponse(new AnswersOnMyQuestionsMenu(this)) { ResultMessage = "Ответ не найден" };
            }
            if (command == "Назад")
            {
                User.CurrentAnswer = null;
                return new CommandResponse(new AnswersOnMyQuestionsMenu(this));
            }
            Db.AnswerReports.Add(new AnswerReport(User.CurrentAnswer.Id, command));
            Db.SaveChanges();
            User.CurrentAnswer = null;
            return new CommandResponse(new AnswersOnMyQuestionsMenu(this)) { ResultMessage = "Жалоба отправлена" };
        }
    }
}
