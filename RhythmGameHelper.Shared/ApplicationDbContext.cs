using Microsoft.EntityFrameworkCore;
using RhythmGameHelper.Shared.DataStructure;

namespace RhythmGameHelper.Shared
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 复合主键定义
            modelBuilder.Entity<SongInclusion>()
                .HasKey(si => new { si.SongId, si.GameId });

            // 唯一索引定义
            modelBuilder.Entity<GameCategory>()
                .HasIndex(gc => new { gc.GameId, gc.Name })
                .IsUnique();

            modelBuilder.Entity<GameVersion>()
                .HasIndex(gc => new { gc.GameId, gc.Name })
                .IsUnique();

            // Game Icollection定义
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Categories)
                .WithOne(gc => gc.Game)
                .HasForeignKey(gc => gc.GameId);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Versions)
                .WithOne(gc => gc.Game)
                .HasForeignKey(gc => gc.GameId);
            
            // 外键定义
            modelBuilder.Entity<GameCategory>()
                .HasMany(gc => gc.Inclusions)
                .WithOne(si => si.Category)
                .HasForeignKey(si => si.CategoryId);

            modelBuilder.Entity<GameVersion>()
                .HasMany(gc => gc.Inclusions)
                .WithOne(si => si.Version)
                .HasForeignKey(si => si.VersionId);
        }

        public DbSet<Song> SongData { get; set; }
        //public DbSet<Chart> ChartData { get; set; }
        public DbSet<Game> GameData { get; set; }
        public DbSet<GameCategory> GameCategories { get; set; }
        public DbSet<GameVersion> GameVersions { get; set; }
        public DbSet<SongInclusion> SongInclusions { get; set; }
    }
}
