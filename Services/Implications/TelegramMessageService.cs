using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace QATopics.Services.Implications
{
    public class TelegramMessageService(ITelegramBotClient botClient) : IMessageService
    {
        private readonly ITelegramBotClient botClient = botClient;
        public async Task SendMessageAsync(long userId, string message)
        {
            await botClient.SendTextMessageAsync(chatId: userId, text: message);
        }
    }
}
