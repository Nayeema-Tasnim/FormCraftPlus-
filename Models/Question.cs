
using System.ComponentModel.DataAnnotations;

namespace finalproject.Models
{
  
    public enum QuestionTypeEnum
    {
         Text,
         Number,
         Checkbox,
         Select
    }

    public enum QuestionState
    {
        Hidden,
        Optional,
        Required
    }

    public class Question
    {
         
         public QuestionTypeEnum QuestionType { get; set; } = QuestionTypeEnum.Text;

         public int Id { get; set; }
        
         public int TemplateId { get; set; }
         public virtual Template Template { get; set; } = default!;
        
         [Required]
         public string? Title { get; set; }
        
         public string? Description { get; set; }
        
         public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

         public QuestionState State { get; set; }
        
         public bool ShowInAnswers { get; set; }
        
      
         public int Order { get; set; }
        
         public string QuestionText { get; set; } = string.Empty;
    }
}


