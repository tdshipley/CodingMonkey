namespace CodingMonkey.Models
{
    using System.IO;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Npgsql.EntityFrameworkCore.PostgreSQL;
    using Microsoft.Extensions.PlatformAbstractions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using System;

    public class CodingMonkeyContext : IdentityDbContext<ApplicationUser>
    {
        public IHostingEnvironment environment { get; set; }
        public IConfiguration configuration { get; set; }

        public CodingMonkeyContext(IHostingEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            this.configuration = configuration;
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseTemplate> ExerciseTemplates { get; set; }
        public DbSet<ExerciseCategory> ExerciseCategories { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestInput> TestInputs { get; set; }
        public DbSet<TestOutput> TestOutputs { get; set; }

        public DbSet<ExerciseExerciseCategory> ExerciseExerciseCategories { get; set; } 

        public CodingMonkeyContext()
        {
            Database.EnsureCreated();
            // Uncomment line below if using migrations (beyond initial)
            // Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (this.environment.IsDevelopment() || this.environment.IsStaging())
            {
                var path = PlatformServices.Default.Application.ApplicationBasePath;
                var connection = $"Filename={Path.Combine(path, "codingmonkey.db")}";
                optionsBuilder.UseSqlite(connection);
            }
            else
            {
                // If in prod use heroku postgres.
                // Database URL configured as environment var by Heroku
                // But needs to be parsed :(
                var connectionString = this.ParsePostqresUriToConnectionString(new Uri(configuration["DATABASE_URL"]));
                optionsBuilder.UseNpgsql(connectionString);
            }

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

        private string ParsePostqresUriToConnectionString(Uri postgresUri)
        {
            string connectionString = string.Empty;

            string server = postgresUri.Host;
            string username = postgresUri.UserInfo.Split(':')[0];
            string password = postgresUri.UserInfo.Split(':')[1];
            string database = postgresUri.AbsolutePath.TrimStart('/');

            connectionString = $"Host={server};Database={database};Username={username};Password={password}";

            return connectionString;
        }
    }
}