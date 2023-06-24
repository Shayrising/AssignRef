using Sabio.Models.Requests.TestQuestionAnswerOptions;
using Sabio.Models.Requests.TestQuestions;

namespace Sabio.Services.Interfaces
{
    public interface ITestQuestionAnswerOptionService
    {
        int Add(TestQuestionAnswerOptionAddRequest model, int userId);

        void Update(TestQuestionAnswerOptionUpdateRequest model, int userId);
        void Delete(int id);
    }
}