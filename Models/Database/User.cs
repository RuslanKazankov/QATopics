using System;
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string Name { get; set; }
        public long UserSettingsId { get; set; }
        public virtual UserSettings? UserSettings { get; set; }
        public int ReportsCount { get; set; }
        public bool Ban { get; set; }
        public virtual Admin? Admin { get; set; }
        public string CurrentMenu { get; set; }
        public virtual List<Question> Questions { get; set; } = [];
        public User(long id, string currentMenu, string name = "Аноним")
        {
            Id = id;
            Name = name;
            CurrentMenu = currentMenu;
        }
    }
}
