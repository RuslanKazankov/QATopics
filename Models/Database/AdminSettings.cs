﻿using System;
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
        public long Id { get; set; }
        public virtual Admin? AdminId { get; set; }
        public int PageOfPopularQuestionsMenu { get; set; } = 0;
        public int PageOfAnswersOnPopularQuestions { get; set; } = 0;
    }
}
