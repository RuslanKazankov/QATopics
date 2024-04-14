using QATopics.Models.Database;
using QATopics.Models.MenuCommands;
using QATopics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Models.Menu
{
    public class MenuParams(User user, IMessageService? messageService) : IMenuParams
    {
        public User User => user;
        public IMessageService? MessageService => messageService;
    }
}
