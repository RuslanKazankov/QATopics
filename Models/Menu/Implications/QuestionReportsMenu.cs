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
            StringBuilder sb = new StringBuilder("Жалобы на вопросы: \n");
            var qreports = Db.QuestionReports.OrderByDescending(qr => qr.Question!.Reports.Count).Take(Config.CountMessagesOnPage / 2).ToList();
            foreach (QuestionReport qreport in qreports)
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
                    QuestionReport? qreport = Db.QuestionReports.Where(qr => qr.Id == qrId).FirstOrDefault();
                    if (qreport != null)
                    {
                        Db.QuestionReports.Remove(qreport);
                        Db.SaveChanges();
                        return new CommandResponse(this) { ResultMessage = "Жалоба отменена" };
                    }
                    return new CommandResponse(this) { ResultMessage = "Жалоба не найдена" };
                }
                return new CommandResponse(this) { ResultMessage = "Некорректный запрос" };
            }
            if (command.StartsWith("/acceptreport"))
            {
                if (int.TryParse(command.Split('_')[1], out int qrId))
                {
                    QuestionReport? qreport = Db.QuestionReports.Where(qr => qr.Id == qrId).FirstOrDefault();
                    if (qreport != null)
                    {
                        Db.Users.Where(u => u.Id == qreport.Question!.UserId).FirstOrDefault()!.ReportsCount++;
                        foreach (var us in Db.UserSettings.Where(us => us.CurrentQuestion != null && us.CurrentQuestion.UserId == qreport.Question.UserId))
                        {
                            us.CurrentQuestion = null;
                        }
                        Db.Questions.Remove(qreport.Question!);
                        Db.SaveChanges();
                        return new CommandResponse(this) { ResultMessage = "Жалоба принята" };
                    }
                    return new CommandResponse(this) { ResultMessage = "Жалоба не найдена" };
                }
                return new CommandResponse(this) { ResultMessage = "Некорректный запрос" };
            }
            if (command.StartsWith("/ban"))
            {
                if (int.TryParse(command.Split('_')[1], out int qrId))
                {
                    QuestionReport? qreport = Db.QuestionReports.Where(qr => qr.Id == qrId).FirstOrDefault();
                    if (qreport != null)
                    {
                        qreport.Question!.User!.ReportsCount++;
                        qreport.Question.User.Ban = true;
                        long userIdBan = qreport.Question.UserId;
                        var questions = Db.Questions.Where(q => q.UserId == qreport.Question.UserId);
                        var answers = Db.Answers.Where(q => q.UserId == qreport.Question.UserId);
                        foreach (var us in Db.UserSettings.Where(us => us.CurrentAnswer != null && us.CurrentAnswer.UserId == qreport.Question.UserId))
                        {
                            us.CurrentAnswer = null;
                        }
                        foreach (var us in Db.UserSettings.Where(us => us.CurrentQuestion != null && us.CurrentQuestion.UserId == qreport.Question.UserId))
                        {
                            us.CurrentQuestion = null;
                        }
                        Db.Questions.RemoveRange(questions);
                        Db.Answers.RemoveRange(answers);
                        Db.SaveChanges();

                        MessageService?.SendMessageAsync(userIdBan, "You have been banned.");
                        return new CommandResponse(this) { ResultMessage = "Пользователь забанен" };
                    }
                    return new CommandResponse(this) { ResultMessage = "Жалоба не найдена" };
                }
                return new CommandResponse(this) { ResultMessage = "Некорректный запрос" };
            }
            return null;
        }
    }
}
