using QATopics.Helpers;
using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using QATopics.Resources;
using QATopics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu.Implications
{
    public class AskQuestionMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetNameOfMenu()
        {
            return nameof(AskQuestionMenu);
        }
        public override string GetMenuText()
        {
            return Replicas.AskQuestionText;
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            return new KeyboardBuilder("Назад").BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "Назад")
                return new CommandResponse(new MainMenu(this));

            if (User.Questions.Count >= Config.MessageCountLimit)
                return new CommandResponse(new MainMenu(this)) { ResultMessage = "У вас слишком много вопросов! Попробуйте удалить неактуальные вопросы." };

            if (command.Length > Config.MessageLengthLimit)
                return new CommandResponse(this) { ResultMessage = $"Длина сообщения ({Config.MessageLengthLimit}) превышена" };

            Question question = new Question(User.Id, command);
            Db.Questions.Add(question);
            Db.SaveChanges();
            return new CommandResponse(new MainMenu(this)) { ResultMessage = "Ваш вопрос добавлен!" };
        }
    }
}
