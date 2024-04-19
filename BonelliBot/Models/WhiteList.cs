using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonelliBot.Models
{
    class WhiteList
    {
        // ریلیشن به به current user لازم است برای ورژن آنلاین
        public int Id { get; set; }
        public long Pk { get; set; }
        public long CurrentUserPk { get; set; }
        public string UserName { get; set; }

    }
}
