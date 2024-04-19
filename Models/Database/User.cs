using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    public class User
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public int? ReportsCount { get; set; }
        public bool Ban { get; set; }
        public string? CurrentMenu { get; set; }
        public long? CurrentQuestionId { get; set; }
        public Question? CurrentQuestion { get; set; }
        public long? AnswerReportId { get; set; }
        public Answer? AnswerReport { get; set; }
        public List<Question> Questions { get; set; } = [];
    }
}
