using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using AssignRef.Models.Domain.TestQuestions;
using AssignRef.Models.Requests.TestQuestions;
using AssignRef.Services;
using AssignRef.Services.Interfaces;
using AssignRef.Web.Controllers;
using AssignRef.Web.Models.Responses;
using System;

namespace AssignRef.Web.Api.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class TestQuestionsApiController : BaseApiController
    {
        private ITestQuestionsService _service = null;
        private IAuthenticationService<int> _authService = null;

        public TestQuestionsApiController(ITestQuestionsService service, IAuthenticationService<int> authenticationService, ILogger<TestQuestionsApiController> logger) : base(logger)
        {
            _service = service;
            _authService = authenticationService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<TestQuestion>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                TestQuestion question = _service.GetById(id);

                if (question == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application Resource not found.");

                }
                else
                {
                    response = new ItemResponse<TestQuestion> { Item = question };
                }
            }
            catch (Exception ex)
            {

                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(TestQuestionRequestBase model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(TestQuestionsUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                _service.Update(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
    }
}

