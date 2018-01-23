namespace CodingMonkey.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Test
    {
        public Test()
        {
            this.TestInputs = new List<TestInput>();
        }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestId { get; set; }
        public string Description { get; set; }
        public List<TestInput> TestInputs { get; set; }
        public TestOutput TestOutput { get; set; }

        public Exercise Exercise { get; set; }

        public void RelateExerciseToTestInMemory(Exercise exercise)
        {
            this.Exercise = exercise;
        }

        public void RelateTestToTestIoInMemory()
        {
            this.RelateTestToTestInputsInMemory();
            this.RelateTestToTestOutputInMemory();
        }

        private void RelateTestToTestOutputInMemory()
        {
            this.TestOutput.Test = this;
            this.TestOutput.Test.TestId = this.TestId;
        }

        private void RelateTestToTestInputsInMemory()
        {
            this.TestInputs.ForEach(testInput => testInput.Test = this);
        }
    }
}