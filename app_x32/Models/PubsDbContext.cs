using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.Entity;
using System.Data.Common;

namespace Cps_x32.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class PubsDbContext : DbContext
    {
        // Constructor to use on a DbConnection that is already opened
        public PubsDbContext(DbConnection existingConnection, bool contextOwnsConnection)
          : base(existingConnection, contextOwnsConnection)
        {

        }

        public DbSet<ArriveStation> ArriveStations { get; set; }
        public DbSet<BreedCoal> BreedCoals { get; set; }
        public DbSet<DispatchStore> DispatchStores { get; set;  }
        public DbSet<LotNumber> LotNumbers { get; set; }
        public DbSet<WeightStation> WeightStations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DispatchStore>().HasKey(ds => new { ds.CarNumber, ds.LotNumberId });
        }
    }
}
