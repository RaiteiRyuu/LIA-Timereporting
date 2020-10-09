using System.Collections.Generic;

namespace Yourworktime.Core.Services
{
    public readonly struct SignUpResult : IResult
    {
        public bool Successful { get; }
        public IEnumerable<string> Errors { get; }

        public SignUpResult(bool successful, IEnumerable<string> errors)
        {
            Successful = successful;
            Errors = errors;
        }
    }
}
