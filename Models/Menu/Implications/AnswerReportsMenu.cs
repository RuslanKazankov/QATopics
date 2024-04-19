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
    internal class AnswerReportsMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            StringBuilder sb = new StringBuilder("Жалобы на ответы: ");
            foreach (AnswerReport areport in PseudoDB.AnswerReports.TakeLast(10))
            {
                sb.Append("Вопрос #").AppendLine(areport.Answer.Question.Id.ToString())
                    .Append("Вопрос: ").AppendLine(areport.Answer.Question.Text);
                sb.Append("Ответ #").AppendLine(areport.Answer.Id.ToString())
                    .Append("Ответ: ").AppendLine(areport.Answer.Text);
                sb.Append("Причина: ").AppendLine(areport.Reason);
                sb.Append("Отказать в жалобе: /cancelreport_").AppendLine(areport.Id.ToString())
                    .Append("Принять жалобу: /acceptreport_").AppendLine(areport.Id.ToString())
                    .Append("Бан: /ban_").AppendLine(areport.Id.ToString());
            }
            return sb.ToString();
        }

        public override string GetNameOfMenu()
        {
            return nameof(AnswerReportsMenu);
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
                PseudoDB.AnswerReports.Remove(PseudoDB.AnswerReports.Where(ar => ar.Id == int.Parse(command.Split('_')[1])).FirstOrDefault());
                return new CommandResponse(new AdminMenu(this));
            }
            if (command.StartsWith("/acceptreport_"))
            {
                AnswerReport ar = PseudoDB.AnswerReports.Where(ar => ar.Id == int.Parse(command.Split('_')[1])).FirstOrDefault();
                PseudoDB.Users.Where(u => u.Id == ar.Answer.Responder.Id).FirstOrDefault().ReportsCount++;
                PseudoDB.Answers.Remove(PseudoDB.Answers.Where(a => a.Id == ar.Answer.Id).FirstOrDefault());
                PseudoDB.AnswerReports.RemoveAll(arw => ar.Answer.Id == arw.Answer.Id);
                return new CommandResponse(new AdminMenu(this));
            }
            if (command.StartsWith("/ban_"))
            {
                AnswerReport ar = PseudoDB.AnswerReports.Where(ar => ar.Id == int.Parse(command.Split('_')[1])).FirstOrDefault();
                PseudoDB.Users.Where(u => u.Id == ar.Answer.Responder.Id).FirstOrDefault().ReportsCount++;
                PseudoDB.Users.Where(u => u.Id == ar.Answer.Responder.Id).FirstOrDefault().Ban = true;
                PseudoDB.Questions.RemoveAll(a => a.User.Id == ar.Answer.Responder.Id);
                PseudoDB.Answers.RemoveAll(a => a.Responder.Id == ar.Answer.Responder.Id);
                PseudoDB.AnswerReports.RemoveAll(a => a.Answer.Responder.Id == ar.Answer.Responder.Id);
                MessageService.SendMessageAsync(ar.Answer.Responder.Id, "You have been banned.");
                return new CommandResponse(new AdminMenu(this));
            }
            return null;
        }
    }
}
