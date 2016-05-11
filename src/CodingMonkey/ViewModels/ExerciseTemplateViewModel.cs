namespace CodingMonkey.ViewModels
{
    public class ExerciseTemplateViewModel
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public string InitialCode { get; set; }
        public string ClassName { get; set; }
        public string MainMethodName { get; set; }
        public string MainMethodSignature { get; set; }
    }
}
