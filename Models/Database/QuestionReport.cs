using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{

    [Table("QuestionReport")]
    public class QuestionReport
    {
        [Column("question_report_id")]
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public virtual Question? Question { get; set; }
        public string Reason { get; set; }
        public QuestionReport(long questionId, string reason)
        {
            QuestionId = questionId;
            Reason = reason;
        }
    }
}
