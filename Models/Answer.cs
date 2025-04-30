namespace finalproject.Models
{
    public class Answer
    {
        public int Id { get; set; }
        
        public int FormId { get; set; }

        public double? NumericValue { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; } = default!;
        
        public string? AnswerText { get; set; }
        
     
        public bool? AnswerCheckbox { get; set; }
         public string Response { get; set; } = string.Empty;
        public int FilledFormId { get; set; }
        public FilledForm FilledForm { get; set; } = null!;
    
    }
}

