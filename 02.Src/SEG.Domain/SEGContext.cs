using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEG.Domain.Model;

namespace SEG.Domain
{
    public class SEGContext : DbContext
    {
        public SEGContext(string connectionString) : base(connectionString)
        {

        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetAudit> AssetAudits { get; set; }
        public DbSet<AssetInfo> AssetInfos { get; set; }
        public DbSet<AssetReport> AssetReports { get; set; }
        public DbSet<Diagnostics> Diagnostics { get; set; }
        public DbSet<DiagnosticsImages> DiagnosticsImages { get; set; }
        public DbSet<DiagnosticsDetail> DiagnosticsDetails { get; set; }
        public DbSet<DiagnosticsDetailInfo> DiagnosticsDetailInfos { get; set; }
        public DbSet<DiagnosticsType> DiagnosticsTypes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemClass> ItemClasses { get; set; }
        public DbSet<ItemInfo> ItemInfos { get; set; }
        public DbSet<ItemSection> ItemSections { get; set; }
        public DbSet<ItemSubSection> ItemSubSections { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectArea> ProjectAreas { get; set; }
        public DbSet<ProjectAreaInfo> ProjectAreaInfos { get; set; }
        public DbSet<ProjectAreaUser> ProjectAreaUsers { get; set; }
        public DbSet<ProjectInfo> ProjectInfos { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<RiskAnalysis> RiskAnalyses { get; set; }
        public DbSet<RiskAnalysisType> RiskAnalysisTypes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>()
            .Configure(c => c.HasColumnType("datetime2"));
        }

    }
}
