using SVLab.Shared.Entities;
using System.Data.Entity;

namespace SVLab.Server.QueryService.Core.DataAccess
{
    public class EntitiesContext : DbContext
    {
        public EntitiesContext()
            : base()
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<MeasurementFile> MeasurementFiles { get; set; }
    }
}