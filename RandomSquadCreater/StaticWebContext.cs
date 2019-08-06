using RandomSquadCreater.Core;
using System.Data.Entity;


namespace RandomSquadCreater
{
    public class StaticWebContext : DbContext
    {
        public StaticWebContext() : base("DefaultConnectionString")
        {
            Database.SetInitializer<StaticWebContext>(new CreateDatabaseIfNotExists<StaticWebContext>());
            //Database.SetInitializer<StaticWebContext>(new MigrateDatabaseToLatestVersion<StaticWebContext, Configuration>());
        }

     
        public virtual DbSet<Score> Score { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Mails> Mails { get; set; }
        public virtual DbSet<Event> Event { get; set; }


    }
}
