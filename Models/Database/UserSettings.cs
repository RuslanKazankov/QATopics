using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    [Table("UserSettings")]
    public class UserSettings
    {
        public long Id { get; set; }
        public virtual User? User { get; set; }
        public int PageOfMyQuestions { get; set; } = 0;
        public int PageOfAnswers{ get; set; } = 0;
        public long? CurrentQuestionId { get; set; }
        public virtual Question? CurrentQuestion { get; set; }
        public long? CurrentAnswerId { get; set; }
        public virtual Answer? CurrentAnswer { get; set; }
    }
}
