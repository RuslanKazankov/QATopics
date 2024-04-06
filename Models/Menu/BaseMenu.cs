using QATopics.Models.MenuCommands;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu
{
    public abstract class BaseMenu(BotUser botUser)
    {
        public BotUser CurrentUser = botUser;
        public abstract string GetMenuText();
        public abstract CommandResponse? SendCommand(string command);
        public abstract ReplyKeyboardMarkup GetRelplyKeyboard();
    }
}
