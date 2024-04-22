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
    public class PopularQuestionsMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            using ApplicationContext db = new ApplicationContext();
            StringBuilder sb = new StringBuilder("Топ самых популярных вопросов:\n");
            foreach (Question question in db.Questions.OrderBy(q => q.LikeCount).TakeLast(20))
            {
                sb.Append("Вопрос #").Append(question.Id)
                    .Append(" задан ").Append(question.User!.Name).Append("#").Append(question.UserId)
                    .Append(" Лайков: ").AppendLine(question.LikeCount.ToString());
                sb.Append("Вопрос: ").AppendLine(question.Text);
                sb.Append("Подробнее: /selectquestion_").AppendLine(question.Id.ToString());
                foreach (Answer answer in question.Answers.Where(a => a.GoodAnswer).TakeLast(5))
                {
                    sb.Append("        Ответ от ").Append(answer.User!.Name).Append("#").AppendLine(answer.UserId.ToString())
                        .AppendLine("        " + answer.Text);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public override string GetNameOfMenu()
        {
            return nameof(PopularQuestionsMenu);
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
            if (command.StartsWith("/selectquestion_"))
            {
                if (int.TryParse(command.Split('_')[1], out int questionId))
                {
                    using ApplicationContext db = new ApplicationContext();
                    User.CurrentQuestion = db.Questions.Where(q => q.Id == questionId).FirstOrDefault();
                    return new CommandResponse(new AnswersOfQuestionAdminMenu(this));
                }
                return new CommandResponse(this) { ResultMessage = "Вопрос не найден"};
            }
            return null;
        }
    }
}
