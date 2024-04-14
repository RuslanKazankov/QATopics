using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using QATopics.Services;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu
{
    public abstract class BaseMenu(User user, IMessageService? messageService) : IMenuParams
    {
        public BaseMenu(IMenuParams menuParams) : this(menuParams.User, menuParams.MessageService) { }
        public User User { get; } = user;
        public IMessageService? MessageService { get; } = messageService;
        public abstract string GetNameOfMenu();
        public abstract string GetMenuText();
        public abstract CommandResponse? SendCommand(string command);
        public abstract ReplyKeyboardMarkup GetRelplyKeyboard();
    }
}
