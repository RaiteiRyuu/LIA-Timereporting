using Yourworktime.Core.Models;
using System.Collections.Generic;

namespace Yourworktime.Core.Services
{
    public readonly struct SignUpResult : IResult
    {
        public bool Successful { get; }
        public IEnumerable<string> Errors { get; }
        public UserModel User { get; }

        public SignUpResult(bool successful, IEnumerable<string> errors, UserModel user)
        {
            Successful = successful;
            Errors = errors;
            User = user;
        }
    }
}
