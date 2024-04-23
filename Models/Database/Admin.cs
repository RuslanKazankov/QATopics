using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QATopics.Models.Database
{
    [Table("Admin")]
    public class Admin
    {
        [Column("admin_id")]
        public long Id { get; set; }
        public long UserId { get; set; }
        public virtual User? User { get; set; }
        public long AdminSettingsId { get; set; }
        public virtual AdminSettings? AdminSettings { get; set; }
        public Admin(long userId, long adminSettingsId)
        {
            UserId = userId;
            AdminSettingsId = adminSettingsId;
        }
    }
}
