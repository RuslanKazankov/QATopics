﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    [Obsolete("Not used any more", true)]
    public static class PseudoDB
    {
        private static Random random = new Random();
        public static List<Answer> Answers { get; set; } = [];
        public static List<User> Users { get; set; } = [];
        public static List<Question> Questions { get; set; } = [];
        public static List<AnswerReport> AnswerReports { get; set; } = [];
        public static List<QuestionReport> QuestionReports { get; set; } = [];
        public static List<Admin> Admins { get; set; } = [];
        public static Question? RandomlyQuestion(User user)
        {
            //List<Question> nouserQuestions = Questions.Where((q) => q.UserId != user.Id).ToList();
            if (Questions.Count > 0)
            {
                return Questions[random.Next(Questions.Count)];
            }
            return null;
        }
    }
}
