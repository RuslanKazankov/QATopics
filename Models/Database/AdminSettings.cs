using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{

    [Table("AdminSettings")]
    public class AdminSettings
    {
        [Column("settings_id")]
        public long Id { get; set; }
        public virtual Admin? AdminId { get; set; }
        public int CountOfSelectedQuestions { get; set; } = 100;
        public int PageOfPopularQuestionsMenu { get; set; } = 0;
    }
}
