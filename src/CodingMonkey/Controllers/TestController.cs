namespace CodingMonkey.Controllers
{
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;
    using Microsoft.AspNet.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Data.Entity;

    [Route("api/exercise/{exerciseId}/[controller]/[action]")]
    public class TestController : Controller
    {
        [FromServices]
        public CodingMonkeyContext CodingMonkeyContext { get; set; }
        
        [HttpGet]
        public JsonResult List(int exerciseId)
        {
            var tests = CodingMonkeyContext
                            .Tests
                            .Include(x => x.TestInputs)
                            .Include(x => x.TestOutput)
                            .Include(x => x.Exercise)
                            .Where(x => x.Exercise.ExerciseId == exerciseId);
            
            List<TestViewModel> result = new List<TestViewModel>();
            
            foreach (var test in tests)
            {
                List<TestInputViewModel> testInputsViewModel = new List<TestInputViewModel>();
                
                foreach (var testInput in test.TestInputs)
                {
                    testInputsViewModel.Add(new TestInputViewModel(){
                        Id = testInput.TestInputId,
                        ArgumentName = testInput.ArgumentName,
                        ValueType = testInput.ValueType,
                        Value = testInput.Value
                    });
                }
                
                result.Add(new TestViewModel(){
                    Id = test.TestId,
                    Description = test.Description,
                    TestInputs = testInputsViewModel,
                    TestOutput = new TestOutputViewModel(){
                       Id = test.TestOutput.TestOutputId,
                       ValueType = test.TestOutput.ValueType,
                       Value = test.TestOutput.Value
                    }     
                });
            }
            
            return Json(result);
        }
        
        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int exerciseId, int id)
        {
            var test = CodingMonkeyContext
                                .Tests
                                .Include(x => x.TestInputs)
                                .Include(x => x.TestOutput)
                                .Include(x => x.Exercise)
                                .SingleOrDefault(e => e.TestId == id && e.Exercise.ExerciseId == exerciseId);
            
            if (test == null)
            {
                return Json(string.Empty);
            }
            
            
            List<TestInputViewModel> testInputsViewModel = new List<TestInputViewModel>();
                
            foreach (var testInput in test.TestInputs)
            {
                testInputsViewModel.Add(new TestInputViewModel(){
                    Id = testInput.TestInputId,
                    ArgumentName = testInput.ArgumentName,
                    ValueType = testInput.ValueType,
                    Value = testInput.Value
                });
            }
            
            var result = new TestViewModel(){
                Id = test.TestId,
                Description = test.Description,
                TestInputs = testInputsViewModel,
                TestOutput = new TestOutputViewModel(){
                    Id = test.TestOutput.TestOutputId,
                    ValueType = test.TestOutput.ValueType,
                    Value = test.TestOutput.Value
                }     
            };
            
            return Json(result);
        }
        
        [HttpPost]
        public JsonResult Create(int exerciseId, [FromBody] TestViewModel model)
        {
            if(model == null)
            {
                return Json(string.Empty);    
            } 
                     
            List<TestInput> testInputs = ConvertTestInputsViewModelToTestInputsModel(model.TestInputs);
            TestOutput testOutput = ConvertTestOutputViewModelToTestOutputModel(model.TestOutput);
            
            Exercise exerciseTestBelongsTo = CodingMonkeyContext
                                                .Exercises
                                                .SingleOrDefault(x => x.ExerciseId == exerciseId);
            
            Test test = new Test()
            {
                Description = model.Description,
                TestInputs = testInputs,
                TestOutput = testOutput,
                Exercise = exerciseTestBelongsTo
            };
            
            try
            {
                CodingMonkeyContext.Tests.Add(test);
                
                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {   
                throw ex;
            }
            
            // Create view model with data from db
            model.Id = test.TestId;
            
            model.TestInputs.Clear();
            // Relate created test inputs
            foreach (var testInput in testInputs)
            {
                testInput.Test = test;
                model.TestInputs.Add(new TestInputViewModel(){
                    Id = testInput.TestInputId,
                    ArgumentName = testInput.ArgumentName,
                    ValueType = testInput.ValueType,
                    Value = testInput.Value
                });
            }
            
            // Relate created test output
            testOutput.TestForeignKey = test.TestId;
            testOutput.Test = test;
            model.TestOutput = new TestOutputViewModel(){
                Id = testOutput.TestOutputId,
                Value = testOutput.Value,
                ValueType = testOutput.ValueType
            };

            try
            {
                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {   
                throw ex;
            }

            return Json(model);
        }
        
        [HttpPost]
        [Route("{id}")]
        public JsonResult Update(int exerciseId, int id, [FromBody] TestViewModel viewModel)
        {
            var exceptionResult = new Dictionary<string, dynamic>();
            var test = CodingMonkeyContext
                                .Tests
                                .Include(x => x.TestInputs)
                                .Include(x => x.TestOutput)
                                .Include(x => x.Exercise)
                                .SingleOrDefault(e => e.TestId == id && e.Exercise.ExerciseId == exerciseId);
            
            if (test == null)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "record not found";
                
                return Json(exceptionResult);
            }

            try
            {   
                test.Description = viewModel.Description;
                
                // Remove deleted test inputs
                var testInputsInDb = CodingMonkeyContext
                                            .TestInputs
                                            .Include(x => x.Test)
                                            .Where(x => x.Test.TestId == id).ToList();
                
                foreach (var testInput in testInputsInDb)
                {
                    if(!viewModel.TestInputs.Any(x => x.Id == testInput.TestInputId))
                    {
                        CodingMonkeyContext.TestInputs.Remove(testInput);
                        test.TestInputs.Remove(testInput);
                    }
                }
                
                // Update existing test inputs
                foreach (var testInputModel in test.TestInputs)
                {
                    // Update test input
                    var updatedTestInput = viewModel.TestInputs.FirstOrDefault(x => x.Id == testInputModel.TestInputId);
                    
                    // Create new test input
                    if (updatedTestInput != null)
                    {
                        testInputModel.ArgumentName = updatedTestInput.ArgumentName;
                        testInputModel.Value = updatedTestInput.Value;
                        testInputModel.ValueType = updatedTestInput.ValueType;
                    }
                }
                
                // Update test outputs
                test.TestOutput.Value = viewModel.TestOutput.Value;
                test.TestOutput.ValueType = viewModel.TestOutput.ValueType;

                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }
                
                // Create new test inputs
                foreach (var newTestInput in viewModel.TestInputs.Where(x => x.Id == null))
                {
                    TestInput testInputToAdd = new TestInput(){
                        ArgumentName = newTestInput.ArgumentName,
                        Value = newTestInput.Value,
                        ValueType = newTestInput.ValueType
                    };
                    
                    test.TestInputs.Add(testInputToAdd);
                    
                    if (ModelState.IsValid)
                    {
                        CodingMonkeyContext.SaveChanges();
                    }
                    
                    newTestInput.Id = testInputToAdd.TestInputId;
                }
            }
            catch (Exception ex)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "exception thrown";
                exceptionResult["ex_object"] = ex;
                
                return Json(exceptionResult);
            }

            return Json(viewModel);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            var result = new Dictionary<string, dynamic>();
            var test = CodingMonkeyContext
                                .Tests
                                .Include(t => t.TestInputs)
                                .Include(t => t.TestOutput)
                                .SingleOrDefault(t => t.TestId == id);
            
            if (test == null)
            {
                result["deleted"] = false;
                result["reason"] = "record not found";
            }
            else
            {
                try
                {
                    // Remove test inputs
                    foreach (var testInput in test.TestInputs)
                    {
                        CodingMonkeyContext.TestInputs.Remove(testInput);
                    }
                    
                    // Remove test output
                    CodingMonkeyContext.TestOutputs.Remove(test.TestOutput);
                    
                    CodingMonkeyContext.Tests.Remove(test);
                    CodingMonkeyContext.SaveChanges();
                    result["deleted"] = true;
                }
                catch(Exception ex)
                {
                    result["deleted"] = false;
                    result["reason"] = "exception thrown";
                }
            }
            
            return Json(result);
        }

        private List<TestInput> ConvertTestInputsViewModelToTestInputsModel(List<TestInputViewModel> testInputViewModel)
        {
            List<TestInput> testInputs = new List<TestInput>();
            
            foreach (var testInput in testInputViewModel)
            {
                testInputs.Add(new TestInput(){
                    Value = testInput.Value,
                    ValueType = testInput.ValueType,
                    ArgumentName = testInput.ArgumentName
                });
            }
            
            return testInputs;
        }
        
        private TestOutput ConvertTestOutputViewModelToTestOutputModel(TestOutputViewModel testOutputViewModel)
        {
            return new TestOutput(){
                Value = testOutputViewModel.Value,
                ValueType = testOutputViewModel.ValueType
            };
        }
    }
}
