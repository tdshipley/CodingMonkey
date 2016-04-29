using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using CodingMonkey.Models;

namespace CodingMonkey.Migrations
{
    [DbContext(typeof(CodingMonkeyContext))]
    [Migration("20160428153607_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("CodingMonkey.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasAnnotation("Relational:Name", "EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .HasAnnotation("Relational:Name", "UserNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetUsers");
                });

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

                    b.Property<string>("Name");

                    b.HasKey("ExerciseCategoryId");
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseExerciseCategory", b =>
                {
                    b.Property<int>("ExerciseId");

                    b.Property<int>("ExerciseCategoryId");

                    b.HasKey("ExerciseId", "ExerciseCategoryId");
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

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasAnnotation("Relational:Name", "RoleNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAnnotation("Relational:TableName", "AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
                });

            modelBuilder.Entity("CodingMonkey.Models.ExerciseExerciseCategory", b =>
                {
                    b.HasOne("CodingMonkey.Models.ExerciseCategory")
                        .WithMany()
                        .HasForeignKey("ExerciseCategoryId");

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

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CodingMonkey.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CodingMonkey.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("CodingMonkey.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}
