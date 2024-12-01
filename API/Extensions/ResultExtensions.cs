using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;

namespace API.Extensions
{
    public static class ResultExtensions
    {
        public static T Match<T>(this Result result,
                Func<T> onSuccess,
                Func<T> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure();
        }
    }
}