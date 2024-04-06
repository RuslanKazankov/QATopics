using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace QATopics.Services
{
    public class TelegramMessageService
    {
        private static TelegramMessageService? instance;
        public bool IsInit { get; private set; }
        private ITelegramBotClient? botClient;
        private TelegramMessageService() { }
        public void Init(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            IsInit = true;
        }
        public static TelegramMessageService GetInstance()
        {
            instance ??= new TelegramMessageService();
            return instance;
        }

        public async void SendMessageToUser(long userId, string message)
        {
            if (botClient == null)
            {
                throw new NullReferenceException(nameof(botClient));
            }
            await botClient.SendTextMessageAsync(chatId: userId, text: message);
        }
    }
}
