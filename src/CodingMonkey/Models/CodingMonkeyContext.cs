namespace CodingMonkey.Models
{
    using System.IO;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.Extensions.PlatformAbstractions;

    public class CodingMonkeyContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseTemplate> ExerciseTemplates { get; set; }
        public DbSet<ExerciseCategory> ExerciseCategories { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestInput> TestInputs { get; set; }
        public DbSet<TestOutput> TestOutputs { get; set; }

        public CodingMonkeyContext()
        {
            Database.EnsureCreated();
            // Uncomment line below if using migrations (beyond initial)
            // Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            var connection = $"Filename={Path.Combine(path, "codingmonkey.db")}";
            optionsBuilder.UseSqlite(connection);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base to create ASP.NET Identity tables first
            base.OnModelCreating(modelBuilder);

            // Exercise Relationships
            modelBuilder.Entity<Exercise>()
                .HasOne(t => t.Template)
                .WithOne(e => e.Exercise)
                .HasForeignKey<ExerciseTemplate>(tfk => tfk.ExerciseForeignKey)
                .OnDelete(DeleteBehavior.Cascade);

            // Exercise / Exercise Category Many to Many Relationships
            // EF 7 Doesn't yet support creating Join tables itself
            // so must do it on the fly.
            modelBuilder.Entity<ExerciseExerciseCategory>()
                .HasKey(t => new { t.ExerciseId, t.ExerciseCategoryId });

            modelBuilder.Entity<ExerciseExerciseCategory>()
                .HasOne(e => e.Exercise)
                .WithMany(ec => ec.ExerciseExerciseCategories)
                .HasForeignKey(eid => eid.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExerciseExerciseCategory>()
                .HasOne(ec => ec.ExerciseCategory)
                .WithMany(c => c.ExerciseExerciseCategories)
                .HasForeignKey(ecid => ecid.ExerciseCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exercise>().HasMany(t => t.Tests).WithOne(e => e.Exercise).OnDelete(DeleteBehavior.Cascade);

            // Test Relationships
            modelBuilder.Entity<Test>()
                .HasOne(to => to.TestOutput)
                .WithOne(t => t.Test)
                .HasForeignKey<TestOutput>(tofk => tofk.TestForeignKey)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Test>().HasMany(ti => ti.TestInputs).WithOne(t => t.Test).OnDelete(DeleteBehavior.Cascade);
        }
    }
}