using AssignRef.Models.Requests.TestQuestionAnswerOptions;
using AssignRef.Models.Requests.TestQuestions;

namespace AssignRef.Services.Interfaces
{
    public interface ITestQuestionAnswerOptionService
    {
        int Add(TestQuestionAnswerOptionAddRequest model, int userId);

        void Update(TestQuestionAnswerOptionUpdateRequest model, int userId);
        void Delete(int id);
    }
}
