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
            using ApplicationContext db = new ApplicationContext();
            foreach (QuestionReport qreport in db.QuestionReports.TakeLast(20))
            {
                sb.Append("Вопрос #").AppendLine(qreport.Question!.Id.ToString())
                    .Append("Вопрос: ").AppendLine(qreport.Question.Text);
                sb.Append("Причина: ").AppendLine(qreport.Reason);
                sb.Append("Количество подтверждённых жалоб на пользователя: ").AppendLine(qreport.Question.User!.ReportsCount.ToString());
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
                if (int.TryParse(command.Split('_')[1], out int qrId))
                {
                    using ApplicationContext db = new ApplicationContext();
                    QuestionReport? qreport = db.QuestionReports.Where(qr => qr.Id == qrId).FirstOrDefault();
                    if (qreport != null)
                    {
                        db.QuestionReports.Remove(qreport);
                        db.SaveChanges();
                        return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба отменена" };
                    }
                    return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба не найдена" };
                }
                return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Некорректный запрос" };
            }
            if (command.StartsWith("/acceptreport"))
            {
                if (int.TryParse(command.Split('_')[1], out int qrId))
                {
                    using ApplicationContext db = new ApplicationContext();
                    QuestionReport? qreport = db.QuestionReports.Where(qr => qr.Id == qrId).FirstOrDefault();
                    if (qreport != null)
                    {
                        db.Users.Where(u => u.Id == qreport.Question!.UserId).FirstOrDefault()!.ReportsCount++;
                        db.Questions.Remove(qreport.Question!);
                        db.SaveChanges();
                        return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба отменена" };
                    }
                    return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба не найдена" };
                }
                return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Некорректный запрос" };
            }
            if (command.StartsWith("/ban"))
            {
                if (int.TryParse(command.Split('_')[1], out int qrId))
                {
                    using ApplicationContext db = new ApplicationContext();
                    QuestionReport? qreport = db.QuestionReports.Where(qr => qr.Id == qrId).FirstOrDefault();
                    if (qreport != null)
                    {
                        qreport.Question!.User!.ReportsCount++;
                        qreport.Question.User.Ban = true;
                        var questions = db.Questions.Where(q => q.UserId == qreport.Question.UserId);
                        var answers = db.Answers.Where(q => q.UserId == qreport.Question.UserId);
                        db.Questions.RemoveRange(questions);
                        db.Answers.RemoveRange(answers);
                        db.SaveChanges();

                        MessageService?.SendMessageAsync(qreport.Question.UserId, "You have been banned.");
                        return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба отменена" };
                    }
                    return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба не найдена" };
                }
                return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Некорректный запрос" };
            }
            return null;
        }
    }
}
