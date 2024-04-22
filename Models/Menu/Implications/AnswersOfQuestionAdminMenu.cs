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
    public class AnswersOfQuestionAdminMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            if (User.CurrentQuestion != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Вопрос #").AppendLine(User.CurrentQuestion.Id.ToString());
                sb.Append("Вопрос от ").Append(User.CurrentQuestion.User!.Name).Append("#").AppendLine(User.CurrentQuestion.User.Id.ToString());
                sb.Append("Вопрос: ").AppendLine(User.CurrentQuestion.Text);
                foreach (Answer answer in User.CurrentQuestion.Answers.TakeLast(50))
                {
                    if (answer.GoodAnswer)
                    {
                        sb.Append("⭐ ");
                    }
                    sb.Append("Ответ #").Append(answer.Id).Append(" от ").AppendLine(answer.User!.Name);
                    sb.AppendLine(answer.Text);
                }
                return sb.ToString();
            }
            return "Вопрос не найден";
        }

        public override string GetNameOfMenu()
        {
            return nameof(AnswersOfQuestionAdminMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            return new KeyboardBuilder("Назад").BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            User.CurrentQuestion = null;
            return new CommandResponse(new PopularQuestionsMenu(this));
        }
    }
}
