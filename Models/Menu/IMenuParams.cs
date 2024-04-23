using QATopics.Models.Database;
using QATopics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Menu
{
    public interface IMenuParams
    {
        public User User { get; }
        public IMessageService? MessageService { get; }
        public ApplicationContext Db { get; }
    }
}
