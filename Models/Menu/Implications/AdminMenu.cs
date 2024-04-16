using QATopics.Helpers;
using QATopics.Models.MenuCommands;
using QATopics.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu.Implications
{
    public class AdminMenu(IMenuParams menuParams) : BaseMenu(menuParams)
    {
        public override string GetMenuText()
        {
            return Replicas.AdminMenuText;
        }

        public override string GetNameOfMenu()
        {
            return nameof(AdminMenu);
        }

        public override ReplyKeyboardMarkup GetRelplyKeyboard()
        {
            return new KeyboardBuilder(["1", "2", "3", "Назад"]).BuildKeyboard();
        }

        public override CommandResponse? SendCommand(string command)
        {
            if (command == "1") //
            {
                return new CommandResponse(new PopularQuestionsMenu(this));
            }
            if (command == "Назад")
            {
                return new CommandResponse(new MainMenu(this));
            }
            return null;
        }
    }
}
