using AssignRef.Models.Requests.TestQuestionAnswerOptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignRef.Models.Requests.TestQuestions
{
    public class TestQuestionRequestBase
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int TestId { get; set; }
        [Required]
        [StringLength(500)]
        public string Question { get; set; }
        [Required]
        [StringLength(255)]
        public string HelpText { get; set; }
        [Required]
        public bool IsRequired { get; set; }
        [Required]
        public bool IsMultipleAllowed { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int QuestionTypeId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int StatusId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int SortOrder { get; set; }
        public List<TestQuestionAnswerOptionAddRequest> AnswerOptions { get; set; }
    }
}
