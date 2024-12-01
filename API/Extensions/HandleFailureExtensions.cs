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
        public static IActionResult HandleFailure(this Result result) =>
            result switch
            {
                { IsSuccess: true } => new BadRequestResult(),

                // if the result is type of IValidationResult instence
                // IValidationResult validationResult =>
                //     new BadRequestObjectResult(
                //         CreateProblemDetails("ValidationError", StatusCodes.Status400BadRequest,
                //             result.Error, validationResult.Errors)
                //     ),

                _ => new BadRequestObjectResult(
                        CreateProblemDetails(result.Error)
                    )
            };


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