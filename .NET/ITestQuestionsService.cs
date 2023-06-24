using AssignRef.Models.Domain.Messages;
using AssignRef.Models.Domain.TestQuestions;
using AssignRef.Models.Requests.TestQuestions;

namespace AssignRef.Services.Interfaces
{
    public interface ITestQuestionsService
    {
        int Add(TestQuestionRequestBase model, int userId);
        void Update(TestQuestionsUpdateRequest model, int userId);
        void Delete(int id);
        TestQuestion GetById(int id);

    }
}
