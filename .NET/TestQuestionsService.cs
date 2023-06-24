using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Requests.TestQuestions;
using Sabio.Services.Interfaces;
using Sabio.Models.Enums;
using Sabio.Models.Requests.TestQuestionAnswerOptions;
using System.Xml.Linq;
using Sabio.Models.Domain.TestQuestions;
using Sabio.Data;

namespace Sabio.Services

{
    public class TestQuestionsService : ITestQuestionsService
    {
        IDataProvider _data = null;

        public TestQuestionsService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(TestQuestionRequestBase model, int userId)
        {

            int id = 0;

                string procName = "[dbo].[TestQuestions_Insert_V2]";
                DataTable myParamValue = MapAnswersToTable(model.AnswerOptions);

                _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@UserId", userId);
                    col.AddWithValue("@TestId", model.TestId);
                    col.AddWithValue("@AnswerOptions_Batch", myParamValue);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                });

            return id;
        }

        private static DataTable MapAnswersToTable(List<TestQuestionAnswerOptionAddRequest> answersToMap)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Columns.Add("AdditionalInfo", typeof(string));
            dt.Columns.Add("IsCorrect", typeof(bool));

            foreach (TestQuestionAnswerOptionAddRequest multAnswer in answersToMap)
            {
                DataRow dr = dt.NewRow();
                int startingIndex = 0;

                dr[startingIndex++] = multAnswer.Text;
                dr[startingIndex++] = multAnswer.Value;
                dr[startingIndex++] = multAnswer.AdditionalInfo;
                dr[startingIndex++] = multAnswer.IsCorrect;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public void Update(TestQuestionsUpdateRequest model, int userId)
        {
            string procName = "[dbo].[TestQuestions_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
            },
            returnParameters: null);
        }

        public TestQuestion GetById(int id)
        {
            TestQuestion question = null;
            string procName = "[dbo].[TestQuestions_Select_ById]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                question = MapSingleQuestion(reader, ref startingIndex);
            });
            return question;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[TestQuestions_Delete_ById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        private static void AddCommonParams(TestQuestionRequestBase model, SqlParameterCollection col)
        {
            col.AddWithValue("@Question", model.Question);
            col.AddWithValue("@HelpText", model.HelpText);
            col.AddWithValue("@IsRequired", model.IsRequired);
            col.AddWithValue("@IsMultipleAllowed", model.IsMultipleAllowed);
            col.AddWithValue("@QuestionTypeId", model.QuestionTypeId);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@SortOrder", model.SortOrder);
        }

        private static TestQuestion MapSingleQuestion(IDataReader reader, ref int startingIndex)
        {
            TestQuestion question = new TestQuestion();

            question.Id = reader.GetSafeInt32(startingIndex++);
            question.Question = reader.GetSafeString(startingIndex++);
            question.HelpText = reader.GetSafeString(startingIndex++);
            question.IsRequired = reader.GetSafeBool(startingIndex++);
            question.IsMultipleAllowed = reader.GetSafeBool(startingIndex++);
            question.QuestionTypeId = reader.GetSafeInt32(startingIndex++);
            question.TestId = reader.GetSafeInt32(startingIndex++);
            question.StatusId = reader.GetSafeInt32(startingIndex++);
            question.SortOrder = reader.GetSafeInt32(startingIndex++);

            return question;
        }
    }
}
