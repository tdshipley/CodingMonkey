using Microsoft.Data.Entity;
using System.Collections.Generic;

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

            modelBuilder.Entity<Exercise>().HasMany(c => c.Categories).WithOne(e => e.Exercise);
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