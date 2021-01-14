using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using Yourworktime.Core;
using Yourworktime.Core.Services;
using Yourworktime.Web.Models;

namespace Yourworktime.Web.Services
{
    public class AuthService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ServerHandler serverHandler;
        private readonly SignUpService signUpService;
        private readonly SignInService signInService;

        public AuthService(AuthenticationStateProvider authenticationStateProvider,
            ServerHandler serverHandler, 
            SignUpService signUpService,
            SignInService signInService)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.serverHandler = serverHandler;
            this.signUpService = signUpService;
            this.signInService = signInService;
        }

        public async Task<SignUpResult> SingUp(SignUpModel signUpModel)
        {
            SignUpResult result = await signUpService.SignUp(ModelConverter.ToUserModel(signUpModel));

            return result;
        }

        public async Task<SignInResult> SignIn(SignInModel signInModel)
        {
            SignInResult signInResult = await signInService.SignIn(signInModel.Email, signInModel.Password);

            if (!signInResult.Successful)
                return signInResult;

            if (signInModel.StaySignedIn)
                await serverHandler.StorageService.UpsertItem("authToken", signInResult.Token);

            ((CustomAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated(signInResult.Token);

            return signInResult;
        }

        public async Task Signout()
        {
            await serverHandler.StorageService.DeleteItem("authToken");
            ((CustomAuthenticationStateProvider)authenticationStateProvider).MarkUserAsSignedOut();
        }
    }
}
