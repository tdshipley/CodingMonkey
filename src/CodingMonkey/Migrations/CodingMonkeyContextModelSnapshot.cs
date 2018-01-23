﻿// <auto-generated />
using CodingMonkey.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CodingMonkey.Migrations
{
    [DbContext(typeof(CodingMonkeyContext))]
    partial class CodingMonkeyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("CodingMonkey.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("CodingMonkey.Models.Exercise", b =>
                {
                    b.Property<int>("ExerciseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Guidance");

                    b.Property<string>("Name");

                    b.HasKey("ExerciseId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseCategory", b =>
                {
                    b.Property<int>("ExerciseCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("ExerciseCategoryId");

                    b.ToTable("ExerciseCategories");
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseExerciseCategory", b =>
                {
                    b.Property<int>("ExerciseId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ExerciseCategoryId");

                    b.HasKey("ExerciseId", "ExerciseCategoryId");

                    b.HasIndex("ExerciseCategoryId");

                    b.ToTable("ExerciseExerciseCategory");
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseTemplate", b =>
                {
                    b.Property<int>("ExerciseTemplateId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassName");

                    b.Property<int>("ExerciseForeignKey");

                    b.Property<string>("InitialCode");

                    b.Property<string>("MainMethodName");

                    b.Property<string>("MainMethodSignature");

                    b.HasKey("ExerciseTemplateId");

                    b.HasIndex("ExerciseForeignKey")
                        .IsUnique();

                    b.ToTable("ExerciseTemplates");
                });

            modelBuilder.Entity("CodingMonkey.Models.Test", b =>
                {
                    b.Property<int>("TestId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("ExerciseId");

                    b.HasKey("TestId");

                    b.HasIndex("ExerciseId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("CodingMonkey.Models.TestInput", b =>
                {
                    b.Property<int>("TestInputId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArgumentName");

                    b.Property<int?>("TestId");

                    b.Property<string>("Value");

                    b.Property<string>("ValueType");

                    b.HasKey("TestInputId");

                    b.HasIndex("TestId");

                    b.ToTable("TestInputs");
                });

            modelBuilder.Entity("CodingMonkey.Models.TestOutput", b =>
                {
                    b.Property<int>("TestOutputId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TestForeignKey");

                    b.Property<string>("Value");

                    b.Property<string>("ValueType");

                    b.HasKey("TestOutputId");

                    b.HasIndex("TestForeignKey")
                        .IsUnique();

                    b.ToTable("TestOutputs");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.Property<string>("ApplicationUserId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseExerciseCategory", b =>
                {
                    b.HasOne("CodingMonkey.Models.ExerciseCategory", "ExerciseCategory")
                        .WithMany("ExerciseExerciseCategories")
                        .HasForeignKey("ExerciseCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodingMonkey.Models.Exercise", "Exercise")
                        .WithMany("ExerciseExerciseCategories")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseTemplate", b =>
                {
                    b.HasOne("CodingMonkey.Models.Exercise", "Exercise")
                        .WithOne("Template")
                        .HasForeignKey("CodingMonkey.Models.ExerciseTemplate", "ExerciseForeignKey")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodingMonkey.Models.Test", b =>
                {
                    b.HasOne("CodingMonkey.Models.Exercise", "Exercise")
                        .WithMany("Tests")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodingMonkey.Models.TestInput", b =>
                {
                    b.HasOne("CodingMonkey.Models.Test", "Test")
                        .WithMany("TestInputs")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodingMonkey.Models.TestOutput", b =>
                {
                    b.HasOne("CodingMonkey.Models.Test", "Test")
                        .WithOne("TestOutput")
                        .HasForeignKey("CodingMonkey.Models.TestOutput", "TestForeignKey")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CodingMonkey.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CodingMonkey.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("CodingMonkey.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodingMonkey.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CodingMonkey.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
