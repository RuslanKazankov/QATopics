using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    [Table("Answer")]
    public class Answer
    {
        [Column("answer_id")]
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public virtual Question? Question { get; set; }
        public string Text { get; set; }
        public long UserId { get; set; }
        public virtual User? User { get; set; }
        public bool GoodAnswer { get; set; }
        public virtual List<AnswerReport> AnswerReports { get; set; } = [];
        public Answer(long questionId, string text, long userId)
        {
            QuestionId = questionId;
            Text = text;
            UserId = userId;
        }
    }
}
