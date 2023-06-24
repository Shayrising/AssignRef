using Sabio.Data.Providers;
using Sabio.Models.Requests.TestQuestions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Requests.TestQuestionAnswerOptions;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class TestQuestionAnswerOptionService : ITestQuestionAnswerOptionService
    {
        IDataProvider _data = null;

        public TestQuestionAnswerOptionService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(TestQuestionAnswerOptionAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[TestQuestionAnswerOptions_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col, userId);

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

        public void Update(TestQuestionAnswerOptionUpdateRequest model, int userId)
        {
            string procName = "[dbo].[TestQuestionAnswerOptions_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col, userId);
                col.AddWithValue("@Id", model.Id);

            },
            returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[TestQuestionAnswerOptions_Delete_ById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            }, returnParameters: null);
        }

        private static void AddCommonParams(TestQuestionAnswerOptionAddRequest model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("@QuestionId", model.QuestionId);
            col.AddWithValue("@Text", model.Text);
            col.AddWithValue("@Value", model.Value);
            col.AddWithValue("@AdditionalInfo", model.AdditionalInfo);
            col.AddWithValue("@CreatedBy", userId);
            col.AddWithValue("@IsCorrect", model.IsCorrect);
        }
    }
}
