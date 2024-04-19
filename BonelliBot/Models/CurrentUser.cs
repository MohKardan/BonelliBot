using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonelliBot.Models
{
    class CurrentUser
    {
        [Key]
        public int Id { get; set; }
        public long Pk { get; set; }
        public long FollowerCount { get; set; }
        public DateTime? FollowBlock { get; set; }
        public DateTime? UnFollowBlock { get; set; }
    }
}
