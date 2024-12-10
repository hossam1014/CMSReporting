using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions
{
    public static class HandleFailureExtensions
    {
        public static ObjectResult HandleFailure(this Result result, int statusCode)
        {
            if (result.IsSuccess) return new ObjectResult("");

            var problem = Results.Problem(statusCode: statusCode);
            
            var problemDetails = problem.GetType().GetProperty("ProblemDetails")?.GetValue(problem) as ProblemDetails;

            problemDetails.Detail = result.Error.Description;
            

            // if the result is type of IValidationResult instence
            // IValidationResult validationResult =>
            //     new BadRequestObjectResult(
            //         CreateProblemDetails("ValidationError", StatusCodes.Status400BadRequest,
            //             result.Error, validationResult.Errors)
            //     ),

            return new BadRequestObjectResult(problemDetails);
        }


        private static ProblemDetails CreateProblemDetails(Error error
                , Error[] errors = null) =>
                new() {
                    Title = error.Code,
                    Detail = error.Description,
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { { nameof(errors), errors } }
                };
    }
}