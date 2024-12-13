using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Error Error { get; } = default!;

        public Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;

            if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            {
                throw new InvalidOperationException();
            }
            
        }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value) => new(true, value, Error.None);

        public static Result<TValue> Failure<TValue>(Error error) => new(false, default, error);


    }
}