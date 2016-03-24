using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using CodingMonkey.Models;

namespace CodingMonkey.Migrations
{
    [DbContext(typeof(CodingMonkeyContext))]
    partial class CodingMonkeyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("CodingMonkey.Models.Exercise", b =>
                {
                    b.Property<int>("ExerciseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Guidance");

                    b.Property<string>("Name");

                    b.HasKey("ExerciseId");
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseCategory", b =>
                {
                    b.Property<int>("ExerciseCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("ExerciseId");

                    b.Property<string>("Name");

                    b.HasKey("ExerciseCategoryId");
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseTemplate", b =>
                {
                    b.Property<int>("ExerciseTemplateId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassName");

                    b.Property<int>("ExerciseForeignKey");

                    b.Property<string>("InitialCode");

                    b.Property<string>("MainMethodName");

                    b.HasKey("ExerciseTemplateId");
                });

            modelBuilder.Entity("CodingMonkey.Models.Test", b =>
                {
                    b.Property<int>("TestId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("ExerciseExerciseId");

                    b.HasKey("TestId");
                });

            modelBuilder.Entity("CodingMonkey.Models.TestInput", b =>
                {
                    b.Property<int>("TestInputId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArgumentName");

                    b.Property<int?>("TestTestId");

                    b.Property<string>("Value");

                    b.Property<string>("ValueType");

                    b.HasKey("TestInputId");
                });

            modelBuilder.Entity("CodingMonkey.Models.TestOutput", b =>
                {
                    b.Property<int>("TestOutputId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TestForeignKey");

                    b.Property<string>("Value");

                    b.Property<string>("ValueType");

                    b.HasKey("TestOutputId");
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseCategory", b =>
                {
                    b.HasOne("CodingMonkey.Models.Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId");
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseTemplate", b =>
                {
                    b.HasOne("CodingMonkey.Models.Exercise")
                        .WithOne()
                        .HasForeignKey("CodingMonkey.Models.ExerciseTemplate", "ExerciseForeignKey");
                });

            modelBuilder.Entity("CodingMonkey.Models.Test", b =>
                {
                    b.HasOne("CodingMonkey.Models.Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseExerciseId");
                });

            modelBuilder.Entity("CodingMonkey.Models.TestInput", b =>
                {
                    b.HasOne("CodingMonkey.Models.Test")
                        .WithMany()
                        .HasForeignKey("TestTestId");
                });

            modelBuilder.Entity("CodingMonkey.Models.TestOutput", b =>
                {
                    b.HasOne("CodingMonkey.Models.Test")
                        .WithOne()
                        .HasForeignKey("CodingMonkey.Models.TestOutput", "TestForeignKey");
                });
        }
    }
}
