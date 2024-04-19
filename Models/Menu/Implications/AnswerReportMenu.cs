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
            if (User.AnswerReport != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Ответ #").AppendLine(User.AnswerReport.Id.ToString())
                    .Append("Ответ: ").AppendLine(User.AnswerReport.Text);
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
            if (User.AnswerReport == null)
            {
                return new CommandResponse(new AnswersOnMyQuestionsMenu(this)) { ResultMessage = "Ответ не найден" };
            }
            if (command == "Назад")
            {
                User.AnswerReport = null;
                return new CommandResponse(new AnswersOnMyQuestionsMenu(this));
            }
            PseudoDB.AnswerReports.Add(new AnswerReport() { Answer = User.AnswerReport, AnswerId = User.AnswerReport.Id, Id = PseudoDB.AnswerReports.LastOrDefault()?.Id + 1 ?? 0, Reason = command });
            User.Questions.Where(q => User.AnswerReport.Question.Id == q.Id).FirstOrDefault()?.Answers.Remove(User.AnswerReport);
            User.AnswerReport = null;
            return new CommandResponse(new AnswersOnMyQuestionsMenu(this)) { ResultMessage = "Жалоба отправлена" };
        }
    }
}
