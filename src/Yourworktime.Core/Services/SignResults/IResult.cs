using System;
using System.Collections.Generic;
using System.Text;

namespace Yourworktime.Core.Services
{
    public interface IResult
    {
        bool Successful { get; }
        IEnumerable<string> Errors { get; }
    }
}
