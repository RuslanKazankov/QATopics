using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    [Table("Question")]
    public class Question
    {
        [Column("question_id")]
        public long Id { get; set; }
        public long UserId { get; set; }
        public virtual User? User { get; set; }
        public string Text { get; set; }
        public int LikeCount { get; set; }
        public virtual List<Answer> Answers { get; set; } = [];
        public virtual List<QuestionReport> Reports { get; set; } = [];
        public DateTime AskDate { get; set; }
        public Question(long userId, string text)
        {
            UserId = userId;
            Text = text;
            AskDate = DateTime.Now;
        }
    }
}
