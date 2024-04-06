using QATopics.Models.Menu;
using QATopics.Models.MenuCommands;
using QATopics.Resources;
using QATopics.Services;
using QATopics.Services.Imp;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models
{
    public class BotUser
    {
        public long ChatId { get; private set; }
        public string Name { get; set; } = "Аноним";
        public BotUser(long chatId)
        {
            ChatId = chatId;
        }
        public BaseMenu GetCurrentMenu()
        {
            return new MainMenu(this);
        }
    }
}
