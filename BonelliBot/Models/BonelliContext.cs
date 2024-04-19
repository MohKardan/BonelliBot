using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using InstagramApiSharp.API;
using System.ComponentModel.DataAnnotations;

namespace BonelliBot.Models
{
    class BonelliContext : DbContext
    {
        static BonelliContext()
        {
            // Not initialize database
            //  Database.SetInitializer<ProjectDatabase>(null);
            // Database initialize
            Database.SetInitializer<BonelliContext>(new DbInitializer());
            using (BonelliContext db = new BonelliContext())
                db.Database.Initialize(false);
        }
        //public DbSet<InstaFriendshipStatus> FriendshipStatuses { get; set; }
        public DbSet<WhiteList> WhiteLists { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<Target> Targets { get; set; }
        //public DbSet<InstaCurrentUser> InstaCurrentUsers { get; set; }
        public DbSet<CurrentUser> CurrentUsers { get; set; }
        //public DbSet<InstaUserInfo> InstaUserInfos { get;set;}
        public DbSet<TargetUnFollow> TargetUnFollows { get; set; }
        //public DbSet<InstaUserShort> InstaUserShorts { get; set; }
        public DbSet<TargetFollower> TargetFollowers { get; set; }
    }
    
    class DbInitializer : DropCreateDatabaseIfModelChanges<BonelliContext>
    {
        protected override void Seed(BonelliContext context)
        {
            // add deafault value in databse
            base.Seed(context);
        }
    }
}
