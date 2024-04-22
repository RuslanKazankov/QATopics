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
            using ApplicationContext db = new ApplicationContext();
            foreach (AnswerReport areport in db.AnswerReports.TakeLast(10))
            {
                sb.Append("Вопрос #").AppendLine(areport.Answer!.Question!.Id.ToString())
                    .Append("Вопрос: ").AppendLine(areport.Answer.Question.Text);
                sb.Append("Ответ #").AppendLine(areport.Answer.Id.ToString())
                    .Append("Ответ: ").AppendLine(areport.Answer.Text);
                sb.Append("Причина: ").AppendLine(areport.Reason);
                sb.Append("Количество подтверждённых жалоб на пользователя: ").AppendLine(areport.Answer.User!.ReportsCount.ToString());
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
                if (int.TryParse(command.Split('_')[1], out int ansrepId))
                {
                    using ApplicationContext db = new ApplicationContext();
                    AnswerReport? answerReport = db.AnswerReports.Where(ar => ar.Id == ansrepId).FirstOrDefault();
                    if (answerReport != null)
                    {
                        db.AnswerReports.Remove(answerReport);
                        db.SaveChanges();
                        return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба отменена" };
                    }
                    return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба не найдена"};
                }
                return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Некорректный запрос"};
            }
            if (command.StartsWith("/acceptreport_"))
            {
                if (int.TryParse(command.Split('_')[1], out int ansrepId))
                {
                    using ApplicationContext db = new ApplicationContext();
                    AnswerReport? ar = db.AnswerReports.Where(ar => ar.Id == ansrepId).FirstOrDefault();
                    if (ar != null)
                    {
                        ar.Answer!.User!.ReportsCount++;
                        db.Answers.Remove(ar.Answer);
                        db.SaveChanges();
                        return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба принята" };
                    }
                    return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба не найдена"};
                }
                return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Некорректный запрос"};
            }
            if (command.StartsWith("/ban_"))
            {
                if (int.TryParse(command.Split('_')[1], out int ansrepId))
                {
                    using ApplicationContext db = new ApplicationContext();
                    AnswerReport? ar = db.AnswerReports.Where(ar => ar.Id == ansrepId).FirstOrDefault();
                    if (ar != null)
                    {
                        ar.Answer!.User!.ReportsCount++;
                        ar.Answer.User.Ban = true;
                        var questions = db.Questions.Where(q => q.UserId == ar.Answer.UserId);
                        db.Questions.RemoveRange(questions);
                        var answers = db.Answers.Where(a => a.UserId == ar.Answer.UserId);
                        db.Answers.RemoveRange(answers);
                        db.SaveChanges();
                        MessageService?.SendMessageAsync(ar.Answer.UserId, "You have been banned.");
                        return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Пользователь забанен"};
                    }
                    return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Жалоба не найдена" };
                }
                return new CommandResponse(new AdminMenu(this)) { ResultMessage = "Некорректный запрос" };
            }
            return null;
        }
    }
}
