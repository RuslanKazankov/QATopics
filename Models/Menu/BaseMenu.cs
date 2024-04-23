using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using QATopics.Services;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu
{
    public abstract class BaseMenu(IMenuParams menuParams) : IMenuParams
    {
        public User User { get; } = menuParams.User;
        public IMessageService? MessageService { get; } = menuParams.MessageService;
        public ApplicationContext Db { get; } = menuParams.Db;
        public abstract string GetNameOfMenu();
        public abstract string GetMenuText();
        public abstract CommandResponse? SendCommand(string command);
        public abstract ReplyKeyboardMarkup GetRelplyKeyboard();
    }
}
