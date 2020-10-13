using System.Collections.Generic;

namespace Yourworktime.Core.Services
{
    public interface IResult
    {
        bool Successful { get; }
        IEnumerable<string> Errors { get; }
    }
}
