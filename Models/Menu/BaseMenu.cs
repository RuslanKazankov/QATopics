using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu
{
    public abstract class BaseMenu(User user)
    {
        public User CurrentUser = user;
        public abstract string GetNameOfMenu();
        public abstract string GetMenuText();
        public abstract CommandResponse? SendCommand(string command);
        public abstract ReplyKeyboardMarkup GetRelplyKeyboard();
    }
}
