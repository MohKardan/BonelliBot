using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonelliBot.Models
{
    class Counter
    {
        [Key]
        public int Id { get; set; }
        public long CurrentUserPk { get; set; }
        public long Follow { get; set; }
        public long UnFollow { get; set; }
        public long FollowBack { get; set; }
        public long Skipped { get; set; }
        public long Reqeusted { get; set; }
    }
}
