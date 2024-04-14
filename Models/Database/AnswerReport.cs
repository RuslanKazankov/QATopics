using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    public class AnswerReport
    {
        public long Id { get; set; }
        public long AnswerId { get; set; }
        public Answer? Answer { get; set; }
        public string? Reason { get; set; }
    }
}
