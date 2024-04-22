using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    [Table("AnswerReport")]
    public class AnswerReport
    {
        [Column("answer_report_id")]
        public long Id { get; set; }
        public long AnswerId { get; set; }
        public virtual Answer? Answer { get; set; }
        public string Reason { get; set; }
        public AnswerReport(long answerId, string reason)
        {
            AnswerId = answerId;
            Reason = reason;
        }
    }
}
