using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonelliBot.Models
{
    public class Target
    {
        [Key]
        public int Id { get; set; }
        public long CurrentUserPk { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
        public string UserName { get; set; }
        public string NextMaxId { get; set; }
        public int Status { get; set; }
    }
}
