using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    public class Report
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public Question? Question { get; set; }
        public string? Reason { get; set; }
    }
}
