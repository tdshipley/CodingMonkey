using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace CodingMonkey.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Guidance = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.ExerciseId);
                });
            migrationBuilder.CreateTable(
                name: "ExerciseCategory",
                columns: table => new
                {
                    ExerciseCategoryId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseCategory", x => x.ExerciseCategoryId);
                });
            migrationBuilder.CreateTable(
                name: "ExerciseTemplate",
                columns: table => new
                {
                    ExerciseTemplateId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassName = table.Column<string>(nullable: true),
                    ExerciseForeignKey = table.Column<int>(nullable: false),
                    InitialCode = table.Column<string>(nullable: true),
                    MainMethodName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTemplate", x => x.ExerciseTemplateId);
                    table.ForeignKey(
                        name: "FK_ExerciseTemplate_Exercise_ExerciseForeignKey",
                        column: x => x.ExerciseForeignKey,
                        principalTable: "Exercise",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    TestId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    ExerciseExerciseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.TestId);
                    table.ForeignKey(
                        name: "FK_Test_Exercise_ExerciseExerciseId",
                        column: x => x.ExerciseExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "ExerciseExerciseCategory",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(nullable: false),
                    ExerciseCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseExerciseCategory", x => new { x.ExerciseId, x.ExerciseCategoryId });
                    table.ForeignKey(
                        name: "FK_ExerciseExerciseCategory_ExerciseCategory_ExerciseCategoryId",
                        column: x => x.ExerciseCategoryId,
                        principalTable: "ExerciseCategory",
                        principalColumn: "ExerciseCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseExerciseCategory_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "TestInput",
                columns: table => new
                {
                    TestInputId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArgumentName = table.Column<string>(nullable: true),
                    TestTestId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestInput", x => x.TestInputId);
                    table.ForeignKey(
                        name: "FK_TestInput_Test_TestTestId",
                        column: x => x.TestTestId,
                        principalTable: "Test",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "TestOutput",
                columns: table => new
                {
                    TestOutputId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TestForeignKey = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestOutput", x => x.TestOutputId);
                    table.ForeignKey(
                        name: "FK_TestOutput_Test_TestForeignKey",
                        column: x => x.TestForeignKey,
                        principalTable: "Test",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ExerciseExerciseCategory");
            migrationBuilder.DropTable("ExerciseTemplate");
            migrationBuilder.DropTable("TestInput");
            migrationBuilder.DropTable("TestOutput");
            migrationBuilder.DropTable("ExerciseCategory");
            migrationBuilder.DropTable("Test");
            migrationBuilder.DropTable("Exercise");
        }
    }
}
