﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    [Table("User")]
    public class User
    {
        [Column("user_id")]
        public long Id { get; set; }
        public string Name { get; set; }
        public int ReportsCount { get; set; }
        public bool Ban { get; set; }
        public virtual Admin? Admin { get; set; }
        public string CurrentMenu { get; set; }
        public long? CurrentQuestionId { get; set; }
        public virtual Question? CurrentQuestion { get; set; }
        public long? CurrentAnswerId { get; set; }
        public virtual Answer? CurrentAnswer { get; set; }
        public virtual List<Question> Questions { get; set; } = [];
        public User(long id, string currentMenu, string name = "Аноним")
        {
            Id = id;
            Name = name;
            CurrentMenu = currentMenu;
        }
    }
}
