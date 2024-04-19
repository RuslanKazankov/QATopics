using Microsoft.EntityFrameworkCore.Diagnostics;
using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu.Implications
{
    public class AnswersOnMyQuestionsMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            List<Answer> answers = PseudoDB.Answers.Where((a) => a.Question.User.Id == User.Id).TakeLast(20).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (Answer answer in answers)
            {
                if (answer.GoodAnswer)
                {
                    sb.Append("⭐ ");
                }
                sb.Append("Ответ #").Append(answer.Id);
                sb.Append("        Отправить жалобу /report_").AppendLine(answer.Id.ToString());
                sb.Append("Ваш вопрос: ").AppendLine(answer.Question.Text);
                sb.Append("Ответ от ").Append(answer.Responder.Name).Append(": ").AppendLine(answer.Text);
                sb.AppendLine();
            }
            sb.AppendLine("Напишите номер ответа, чтобы пометить ответ хорошим/снять метку хорошего ответа.");
            return sb.ToString();
        }

        public override string GetNameOfMenu()
        {
            return nameof(AnswersOnMyQuestionsMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            ReplyKeyboardMarkup replyKeyboard = new(new KeyboardButton[] {
                new KeyboardButton("Назад")
            });
            replyKeyboard.ResizeKeyboard = true;
            return replyKeyboard;
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "Назад")
            {
                return new CommandResponse(new MainMenu(this));
            }
            if (command.StartsWith("/report_"))
            {
                if (command.Split(' ').Length > 0 && int.TryParse(command.Split('_')[1], out int idOfReport))
                {
                    Answer? answer = PseudoDB.Answers.Where((a) => a.Question.UserId == User.Id && a.Id == idOfReport).FirstOrDefault();
                    if (answer == null)
                    {
                        return new CommandResponse(this) { ResultMessage = "Ответ не найден!" };
                    }
                    User.AnswerReport = answer;
                    return new CommandResponse(new AnswerReportMenu(this));
                }
                return null;
            }
            if (int.TryParse(command, out int idOfAnswer))
            {
                Answer? answer = PseudoDB.Answers.Where((a) => a.Question.UserId == User.Id && a.Id == idOfAnswer).FirstOrDefault();
                if (answer == null)
                {
                    return new CommandResponse(this) { ResultMessage = "Ответ не найден!"};
                }
                answer.GoodAnswer = !answer.GoodAnswer;
                return new CommandResponse(this) { ResultMessage = "Ответ #" + answer.Id + " оценен!" };
            }
            return null;
        }
    }
}
