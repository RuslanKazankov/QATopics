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
    public class QuestionReportsMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            StringBuilder sb = new StringBuilder("Жалобы на вопросы: ");
            foreach (QuestionReport qreport in PseudoDB.QuestionReports.TakeLast(20))
            {
                sb.Append("Вопрос #").AppendLine(qreport.Question.Id.ToString())
                    .Append("Вопрос: ").AppendLine(qreport.Question.Text);
                sb.Append("Причина: ").AppendLine(qreport.Reason);
                sb.Append("Отказать в жалобе: /cancelreport_").AppendLine(qreport.Id.ToString())
                    .Append("Принять жалобу: /acceptreport_").AppendLine(qreport.Id.ToString())
                    .Append("Бан: /ban_").AppendLine(qreport.Id.ToString());
            }
            return sb.ToString();
        }

        public override string GetNameOfMenu()
        {
            return nameof(QuestionReportsMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            return new KeyboardBuilder("Назад").BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "Назад")
            {
                return new CommandResponse(new AdminMenu(this));
            }
            if (command.StartsWith("/cancelreport_"))
            {
                int qrId = int.Parse(command.Split('_')[1]);
                QuestionReport qreport = PseudoDB.QuestionReports.Where(qr => qr.Id == qrId).FirstOrDefault();
                PseudoDB.QuestionReports.Remove(qreport);
                return new CommandResponse(new AdminMenu(this));
            }
            if (command.StartsWith("/acceptreport"))
            {
                int qrId = int.Parse(command.Split('_')[1]);
                QuestionReport qreport = PseudoDB.QuestionReports.Where(qr => qr.Id == qrId).FirstOrDefault();
                PseudoDB.Users.Where(u => u.Id == qreport.Question.User.Id).FirstOrDefault().ReportsCount++;
                PseudoDB.Questions.Remove(qreport.Question);
                PseudoDB.QuestionReports.Remove(qreport);
                return new CommandResponse(new AdminMenu(this));
            }
            if (command.StartsWith("/ban"))
            {
                int qrId = int.Parse(command.Split('_')[1]);
                QuestionReport qreport = PseudoDB.QuestionReports.Where(qr => qr.Id == qrId).FirstOrDefault();
                PseudoDB.Users.Where(u => u.Id == qreport.Question.User.Id).FirstOrDefault().ReportsCount++;
                PseudoDB.Users.Where(u => u.Id == qreport.Question.User.Id).FirstOrDefault().Ban = true;
                PseudoDB.Questions.RemoveAll(q=> q.User.Id == qreport.Question.User.Id);
                PseudoDB.Answers.RemoveAll(a=> a.Responder.Id == qreport.Question.User.Id);
                PseudoDB.QuestionReports.RemoveAll(qr => qr.Question.User.Id == qreport.Question.User.Id);
                MessageService.SendMessageAsync(qreport.Question.User.Id, "You have been banned.");
                return new CommandResponse(new AdminMenu(this));
            }
            return null;
        }
    }
}
