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
    public class ChangeNameMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetNameOfMenu()
        {
            return nameof(ChangeNameMenu);
        }

        public override string GetMenuText()
        {
            return "✅ Ваше текущее имя: " + User.Name + "\n✏ " + Replicas.ChangeNameText;
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
                return new CommandResponse(new MainMenu(this));

            if (command.Length > Config.NameLengthLimit)
                return new CommandResponse(this) { ResultMessage = $"Лимит имени ({Config.NameLengthLimit}) превышен." };

            User.Name = command;
            return new CommandResponse(new MainMenu(this)) { ResultMessage = "Ваш имя обновлено!" };
        }
    }
}
