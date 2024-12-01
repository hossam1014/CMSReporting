using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public class Result<TValue> : Result
    {
        private readonly TValue _value;

        public Result(bool isSuccess, TValue value, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        
        public TValue Value => IsSuccess
            ? _value
            : throw new InvalidOperationException("the value of a failure result cant be reached ");

    }
}