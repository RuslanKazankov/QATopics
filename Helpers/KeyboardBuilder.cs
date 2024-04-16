using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace QATopics.Helpers
{
    public class KeyboardBuilder
    {
        private List<KeyboardButton> keyboardButtons = [];
        public KeyboardBuilder(string button)
        {
            AddKeyboardButton(button);
        }
        public KeyboardBuilder(IEnumerable<string> buttons)
        {
            AddKeyboardButton(buttons);
        }

        public void AddKeyboardButton(string button)
        {
            keyboardButtons.Add(new KeyboardButton(button));
        }

        public void AddKeyboardButton(IEnumerable<string> buttons)
        {
            foreach (string button in buttons)
            {
                keyboardButtons.Add(new KeyboardButton(button));
            }
        }

        public ReplyKeyboardMarkup BuildKeyboard() {
            return new ReplyKeyboardMarkup(keyboardButtons) { ResizeKeyboard = true };
        }
    }
}
