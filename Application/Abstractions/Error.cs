using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public record Error(string Code, string Description)
    {
        public static readonly Error None = new Error(string.Empty, string.Empty);
    }
}