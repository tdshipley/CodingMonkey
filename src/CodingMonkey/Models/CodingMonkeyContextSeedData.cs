namespace CodingMonkey.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using CodingMonkey.ViewModels;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Mvc.ModelBinding;

    public class CodingMonkeyContextSeedData
    {
        private CodingMonkeyContext _context;

        private UserManager<ApplicationUser> _userManager;

        public CodingMonkeyContextSeedData(CodingMonkeyContext context, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task EnsureSeedDataAsync()
        {
            if (await this._userManager.FindByEmailAsync("thomas.shipley@googlemail.com") == null)
            {
                // Add user
                var user = new ApplicationUser() { UserName = "tdshipley", Email = "thomas.shipley@googlemail.com" };

                await this._userManager.CreateAsync(user, "password!");
            }

            if (this._context.Exercises.FirstOrDefault() == null)
            {
                // Create Exercise Categories
                string integerManipulationCategoryDescription =
                    "A collection of exercises focusing on working with integers in C#. Exercises suitable for beginners and the more experienced.";

                ExerciseCategory integerManipulationCategory = new ExerciseCategory()
                                                                   {
                                                                       Name = "Integer Manipulation",
                                                                       Description =
                                                                           integerManipulationCategoryDescription
                                                                   };

                string stringManipulationCategoryDescription =
                    "A collection of exercises focusing on working with strings in C#. Exercises suitable for beginners and the more experienced.";

                ExerciseCategory stringManipulationCategory = new ExerciseCategory()
                                                                  {
                                                                      Name = "String Manipulation",
                                                                      Description =
                                                                          stringManipulationCategoryDescription
                                                                  };

                this._context.ExerciseCategories.AddRange(
                    new List<ExerciseCategory>() { stringManipulationCategory, integerManipulationCategory });

                // Create Exercises
                Exercise addTwoNumbersExercise = new Exercise()
                                                     {
                                                         Name = "Add Two Numbers",
                                                         Guidance =
                                                             "Complete the code to add two numbers together (a & b) and return the result."
                                                     };

                Exercise multiplyTwoNumbersExercise = new Exercise()
                                                          {
                                                              Name = "Multiply Two Numbers",
                                                              Guidance =
                                                                  "Complete the code to multiply two numbers together (a & b) and return the result."
                                                          };

                Exercise findFindFirstLetterExercise = new Exercise()
                                                           {
                                                               Name = "Get First Letter of a String",
                                                               Guidance =
                                                                   "Complete the GetFirstLetter method to return the first letter of the string argument 'input'."
                                                           };

                this._context.Exercises.AddRange(
                    new List<Exercise>()
                        {
                            addTwoNumbersExercise,
                            multiplyTwoNumbersExercise,
                            findFindFirstLetterExercise
                        });

                // Save Exercise Categories and Exercises to DB
                try
                {
                    this._context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                // Assign Exercises to Exercise Categories
                AssignExerciseToCategoryInMemory(addTwoNumbersExercise, integerManipulationCategory);
                AssignExerciseToCategoryInMemory(multiplyTwoNumbersExercise, integerManipulationCategory);

                AssignExerciseToCategoryInMemory(findFindFirstLetterExercise, stringManipulationCategory);

                // Add Exercise Templates
                var addTwoNumbersExerciseTemplate = new ExerciseTemplate()
                                                        {
                                                            ClassName = "Calculator",
                                                            MainMethodName = "Add",
                                                            InitialCode = "public class Calculator\n" +
                                                                          "{\n" +
                                                                          "    public int Add(int a, int b)\n" +
                                                                          "    {\n" +
                                                                          "        // Your Code!\n" +
                                                                          "    }\n" +
                                                                          "}"
                };

                AssignExerciseToExerciseTemplateInMemory(addTwoNumbersExercise, addTwoNumbersExerciseTemplate);

                var multiplyTwoNumbersExerciseTemplate = new ExerciseTemplate()
                                                             {
                                                                 ClassName = "Calculator",
                                                                 MainMethodName = "Multiply",
                                                                 InitialCode = "public class Calculator\n" +
                                                                               "{\n" +
                                                                               "    public int Multiply(int a, int b)\n" +
                                                                               "    {\n" +
                                                                               "        // Your Code!\n" +
                                                                               "    }\n" +
                                                                               "}"
                };

                AssignExerciseToExerciseTemplateInMemory(multiplyTwoNumbersExercise, multiplyTwoNumbersExerciseTemplate);

                var findFirstLetterExerciseTemplate = new ExerciseTemplate()
                                                          {
                                                              ClassName = "StringManipulation",
                                                              MainMethodName = "GetFirstLetter",
                                                              InitialCode = "public class StringManipulation\n" +
                                                                            "{\n" +
                                                                            "    public string GetFirstLetter(string input)\n" +
                                                                            "    {\n" +
                                                                            "        // Your Code!\n" +
                                                                            "    }\n" +
                                                                            "}"
                };

                AssignExerciseToExerciseTemplateInMemory(findFindFirstLetterExercise, findFirstLetterExerciseTemplate);

                this._context.ExerciseTemplates.AddRange(new List<ExerciseTemplate>()
                                                             {
                                                                 addTwoNumbersExerciseTemplate,
                                                                 multiplyTwoNumbersExerciseTemplate,
                                                                 findFirstLetterExerciseTemplate
                                                             });


                // Add Tests
                Test addNumbersZeroInputTest = new Test()
                                                   {
                                                       Description = "When a = 0 & b = 0 \"Add\" returns 0.",
                                                       Exercise = addTwoNumbersExercise
                                                   };

                Test addNumbersSmallInputTest = new Test()
                                                    {
                                                        Description = "When a = 2 & b = 2 \"Add\" returns 4.",
                                                        Exercise = addTwoNumbersExercise
                                                    };

                Test addNumbersLargeInputTest = new Test()
                                                    {
                                                        Description = "When a = 120 & b = 125 \"Add\" returns 245.",
                                                        Exercise = addTwoNumbersExercise
                                                    };

                Test multiplyNumbersZeroInputTest = new Test()
                                                        {
                                                            Description =
                                                                "When a = 0 & b = 0 \"Multiply\" returns 0.",
                                                            Exercise = multiplyTwoNumbersExercise
                                                        };

                Test multiplyNumbersSmallInputTest = new Test()
                                                         {
                                                             Description =
                                                                 "When a = 10 & b = 2 \"Multiply\" returns 20.",
                                                             Exercise = multiplyTwoNumbersExercise
                                                         };

                Test multiplyNumbersLargeInputTest = new Test()
                                                         {
                                                             Description =
                                                                 "When a = 26 & b = 1024 \"Multiply\" returns 26624.",
                                                             Exercise = multiplyTwoNumbersExercise
                                                         };

                Test findFirstLetterFirstTest = new Test()
                                                    {
                                                        Description =
                                                            "If 'input' is \"Monkey\" it should return \"M\".",
                                                        Exercise = findFindFirstLetterExercise
                                                    };

                Test findFirstLetterSecondTest = new Test()
                {
                    Description = "If 'input' is \"doNkey\" it should return \"d\".",
                    Exercise = findFindFirstLetterExercise
                };

                this._context.Tests.AddRange(new List<Test>()
                                                 {
                                                     addNumbersZeroInputTest,
                                                     addNumbersSmallInputTest,
                                                     addNumbersLargeInputTest,
                                                     multiplyNumbersZeroInputTest,
                                                     multiplyNumbersSmallInputTest,
                                                     multiplyNumbersLargeInputTest,
                                                     findFirstLetterFirstTest,
                                                     findFirstLetterSecondTest
                                                 });

                // Save Updated Exercises, Exercise Template and Tests to the DB
                try
                {
                    this._context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                // #### Add Number Tests
                var addZeroTestInputs = new List<TestInput>()
                                            {
                                                new TestInput()
                                                    {
                                                        ArgumentName = "a",
                                                        Value = "0",
                                                        ValueType = "Integer"
                                                    },
                                                new TestInput()
                                                    {
                                                        ArgumentName = "b",
                                                        Value = "0",
                                                        ValueType = "Integer"
                                                    }
                                            };

                var addZeroTestOutput = new TestOutput() { Value = "0", ValueType = "Integer" };

                AssignTestInputAndOutputToTestInMemory(addNumbersZeroInputTest, addZeroTestInputs, addZeroTestOutput);

                var addSmallTestInputs = new List<TestInput>()
                                             {
                                                 new TestInput()
                                                     {
                                                         ArgumentName = "a",
                                                         Value = "2",
                                                         ValueType = "Integer"
                                                     },
                                                 new TestInput()
                                                     {
                                                         ArgumentName = "b",
                                                         Value = "2",
                                                         ValueType = "Integer"
                                                     }
                                             };

                var addSmallTestOutput = new TestOutput() { Value = "4", ValueType = "Integer" };

                AssignTestInputAndOutputToTestInMemory(addNumbersSmallInputTest, addSmallTestInputs, addSmallTestOutput);

                var addLargeTestInputs = new List<TestInput>()
                                             {
                                                 new TestInput()
                                                     {
                                                         ArgumentName = "a",
                                                         Value = "120",
                                                         ValueType = "Integer"
                                                     },
                                                 new TestInput()
                                                     {
                                                         ArgumentName = "b",
                                                         Value = "125",
                                                         ValueType = "Integer"
                                                     }
                                             };

                var addLargeTestOutput = new TestOutput() { Value = "245", ValueType = "Integer" };

                AssignTestInputAndOutputToTestInMemory(addNumbersLargeInputTest, addLargeTestInputs, addLargeTestOutput);

                // #### Multiply Numbers Tests
                var multiplyZeroTestInputs = new List<TestInput>()
                                                 {
                                                     new TestInput()
                                                         {
                                                             ArgumentName = "a",
                                                             Value = "0",
                                                             ValueType = "Integer"
                                                         },
                                                     new TestInput()
                                                         {
                                                             ArgumentName = "b",
                                                             Value = "0",
                                                             ValueType = "Integer"
                                                         }
                                                 };

                var multiplyZeroTestOutput = new TestOutput() { Value = "0", ValueType = "Integer" };

                AssignTestInputAndOutputToTestInMemory(
                    multiplyNumbersZeroInputTest,
                    multiplyZeroTestInputs,
                    multiplyZeroTestOutput);

                multiplyNumbersSmallInputTest.TestInputs = new List<TestInput>()
                                                               {
                                                                   new TestInput()
                                                                       {
                                                                           Test =
                                                                               multiplyNumbersSmallInputTest,
                                                                           ArgumentName =
                                                                               "a",
                                                                           Value = "2",
                                                                           ValueType =
                                                                               "Integer"
                                                                       },
                                                                   new TestInput()
                                                                       {
                                                                           Test =
                                                                               multiplyNumbersSmallInputTest,
                                                                           ArgumentName =
                                                                               "b",
                                                                           Value = "2",
                                                                           ValueType =
                                                                               "Integer"
                                                                       }
                                                               };

                multiplyNumbersSmallInputTest.TestOutput = new TestOutput()
                                                               {
                                                                   Test = multiplyNumbersSmallInputTest,
                                                                   TestForeignKey =
                                                                       multiplyNumbersSmallInputTest.TestId,
                                                                   Value = "4",
                                                                   ValueType = "Integer"
                                                               };

                var multiplyLargeTestInputs = new List<TestInput>()
                                                  {
                                                      new TestInput()
                                                          {
                                                              ArgumentName = "a",
                                                              Value = "26",
                                                              ValueType = "Integer"
                                                          },
                                                      new TestInput()
                                                          {
                                                              ArgumentName = "b",
                                                              Value = "1024",
                                                              ValueType = "Integer"
                                                          }
                                                  };

                var multiplyLargeTestOutput = new TestOutput() { Value = "26624", ValueType = "Integer" };

                AssignTestInputAndOutputToTestInMemory(
                    multiplyNumbersLargeInputTest,
                    multiplyLargeTestInputs,
                    multiplyLargeTestOutput);

                // #### Find first letter in string tests
                var findFirstLetterFirstTestInputs = new List<TestInput>()
                                                         {
                                                             new TestInput()
                                                                 {
                                                                     ArgumentName = "input",
                                                                     Value = "Monkey",
                                                                     ValueType = "String"
                                                                 }
                                                         };

                var findFirstLetterFirstTestOutput = new TestOutput() { Value = "M", ValueType = "String" };
                AssignTestInputAndOutputToTestInMemory(findFirstLetterFirstTest, findFirstLetterFirstTestInputs, findFirstLetterFirstTestOutput);

                var findFirstLetterSecondTestInputs = new List<TestInput>()
                                                         {
                                                             new TestInput()
                                                                 {
                                                                     ArgumentName = "input",
                                                                     Value = "doNkey",
                                                                     ValueType = "String"
                                                                 }
                                                         };

                var findFirstLetterSecondTestOutput = new TestOutput() { Value = "d", ValueType = "String" };
                AssignTestInputAndOutputToTestInMemory(findFirstLetterSecondTest, findFirstLetterSecondTestInputs, findFirstLetterSecondTestOutput);

                // Save Tests
                try
                {
                    this._context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        private static void AssignExerciseToCategoryInMemory(Exercise exercise, ExerciseCategory exerciseCategory)
        {
            exercise.ExerciseExerciseCategories = new List<ExerciseExerciseCategory>()
                                                                            {
                                                                                new ExerciseExerciseCategory
                                                                                    ()
                                                                                    {
                                                                                        ExerciseId
                                                                                            = exercise.ExerciseId,
                                                                                        ExerciseCategoryId
                                                                                            = exerciseCategory.ExerciseCategoryId
                                                                                    }
                                                                            };
        }

        private static void AssignExerciseToExerciseTemplateInMemory(Exercise exercise, ExerciseTemplate template)
        {
            template.ExerciseForeignKey = exercise.ExerciseId;
            template.Exercise = exercise;

        }

        private static void AssignTestInputAndOutputToTestInMemory(Test test, List<TestInput> testInputs, TestOutput testOutput)
        {
            foreach (var testInput in testInputs)
            {
                AssignTestInputToTestInMemory(test, testInput);
            }

            AssignTestToTestOutputInMemory(test, testOutput);
        }

        private static void AssignTestInputToTestInMemory(Test test, TestInput testInput)
        {
            test.TestInputs = test.TestInputs ?? new List<TestInput>();
            testInput.Test = test;
            test.TestInputs.Add(testInput);
        }

        private static void AssignTestToTestOutputInMemory(Test test, TestOutput testOutput)
        {
            test.TestOutput = testOutput;
            testOutput.Test = test;
            testOutput.TestForeignKey = test.TestId;
        }
    }
}
