using System.Collections.Generic;

namespace Yourworktime.Core.Services
{
    public readonly struct SignupResult : IResult
    {
        public bool Successful { get; }
        public IEnumerable<string> Errors { get; }

        public SignupResult(bool successful, IEnumerable<string> errors)
        {
            Successful = successful;
            Errors = errors;
        }
    }
}
