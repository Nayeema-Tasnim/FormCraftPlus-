using System.Collections.Generic;

namespace finalproject.Models
{
    public class FormViewModel
    {
        public int TemplateId { get; set; }
        public string? TemplateTitle { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();
    }

    public class AnswerViewModel
    {
        public int QuestionId { get; set; }
        public string? AnswerText { get; set; }
    }
}
