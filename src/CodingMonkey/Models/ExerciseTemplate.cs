namespace CodingMonkey.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ExerciseTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExerciseTemplateId { get; set; }
        public string InitialCode { get; set; }
        public string ClassName { get; set; }
        public string MainMethodName { get; set; }
        public string MainMethodSignature { get; set; }

        public int ExerciseForeignKey { get; set; }
        public Exercise Exercise { get; set; }
    }
}