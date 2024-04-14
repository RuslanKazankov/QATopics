using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    public class Question
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User? User { get; set; }
        public string? Text { get; set; }
        public int LikeCount { get; set; }
        public List<Answer> Answers { get; set; } = [];
        public List<QuestionReport> Reports { get; set; } = [];
    }
}
