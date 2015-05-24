using System.Configuration;
using System.Data.Entity;
using System.Data.SQLite;

namespace eve_log_watcher.model
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base(new SQLiteConnection(ConfigurationManager.ConnectionStrings["eve-log-watcher"].ConnectionString), true) {
        }

        // ReSharper disable UnusedMember.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public DbSet<Region> Regions { get; set; }
        public DbSet<SolarSystem> SolarSystems { get; set; }
        public DbSet<SolarSystemJump> SolarSystemJumps { get; set; }
        // ReSharper restore UnusedMember.Global
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}
