using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.TestQuestionAnswerOptions
{
    public class TestQuestionAnswerOptionAddRequest
    {
        public int QuestionId { get; set; }

        [AllowNull]
        [StringLength(500)]
        public string Text { get; set; }

        [StringLength(100)]
        [AllowNull]
        public string Value { get; set; }

        [AllowNull]
        [StringLength(200)]
        public string AdditionalInfo { get; set; }

        public bool IsCorrect { get; set; }
    }
}
