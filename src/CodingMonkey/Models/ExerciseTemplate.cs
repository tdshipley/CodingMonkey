namespace CodingMonkey.Models
{

    public class ExerciseTemplate
    {
        public int ExerciseTemplateId { get; set; }
        public string InitialCode { get; set; }
        public string ClassName { get; set; }
        public string MainMethodName { get; set; }
        public string MainMethodSignature { get; set; }

        public int ExerciseForeignKey { get; set; }
        public Exercise Exercise { get; set; }
    }
}