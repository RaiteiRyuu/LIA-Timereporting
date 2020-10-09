using System.Collections.Generic;

namespace Yourworktime.Core.Services
{
    public readonly struct SignInResult : IResult
    {
        public bool Successful { get; }
        public IEnumerable<string> Errors { get; }
        public string Token { get; }

        public SignInResult(bool succsessful, IEnumerable<string> errors, string token)
        {
            Successful = succsessful;
            Errors = errors;
            Token = token;
        }
    }
}
