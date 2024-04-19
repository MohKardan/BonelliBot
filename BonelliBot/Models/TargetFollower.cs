using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonelliBot.Models
{
    class TargetFollower
    {
        [Key]
        public int Id { get; set; }
        public long Pk { get; set; }
        public long CurrentUserPk { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public bool IsFollowed { get; set; } = false;
    }
}
