using Microsoft.Data.Entity;

namespace CodingMonkey.Models
{
    public class CodingMonkeyContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseTemplate> ExerciseTemplates { get; set; }
        public DbSet<ExerciseCategory> ExerciseCategories { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestInput> TestInputs { get; set; }
        public DbSet<TestOutput> TestOutputs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Exercise Relationships
            modelBuilder.Entity<Exercise>()
                .HasOne(t => t.Template)
                .WithOne(e => e.Exercise)
                .HasForeignKey<ExerciseTemplate>(tfk => tfk.ExerciseForeignKey);

            // Exercise / Exercise Category Many to Many Relationships
            // EF 7 Doesn't yet support creating Join tables itself
            // so must do it on the fly.
            modelBuilder.Entity<ExerciseExerciseCategory>()
                .HasKey(t => new { t.ExerciseId, t.ExerciseCategoryId });

            modelBuilder.Entity<ExerciseExerciseCategory>()
                .HasOne(e => e.Exercise)
                .WithMany(ec => ec.ExerciseExerciseCategories)
                .HasForeignKey(eid => eid.ExerciseId);

            modelBuilder.Entity<ExerciseExerciseCategory>()
                .HasOne(ec => ec.ExerciseCategory)
                .WithMany(c => c.ExerciseExerciseCategories)
                .HasForeignKey(ecid => ecid.ExerciseCategoryId);

            modelBuilder.Entity<Exercise>().HasMany(t => t.Tests).WithOne(e => e.Exercise);

            // Test Relationships
            modelBuilder.Entity<Test>()
                .HasOne(to => to.TestOutput)
                .WithOne(t => t.Test)
                .HasForeignKey<TestOutput>(tofk => tofk.TestForeignKey);

            modelBuilder.Entity<Test>().HasMany(ti => ti.TestInputs).WithOne(t => t.Test);
        }
    }
}