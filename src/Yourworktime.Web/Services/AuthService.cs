using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Yourworktime.Core.Services;
using Yourworktime.Web.Models;

namespace Yourworktime.Web.Services
{
    public class AuthService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ILocalStorageService localStorage;
        private readonly SignUpService signUpService;
        private readonly SignInService signInService;

        public AuthService(AuthenticationStateProvider authenticationStateProvider, 
            ILocalStorageService localStorage, 
            SignUpService signUpService,
            SignInService signInService)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.localStorage = localStorage;
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
                await localStorage.SetItemAsync("authToken", signInResult.Token);

            ((CustomAuthenticationStateProvider)authenticationStateProvider).MarkUserAsAuthenticated(signInResult.Token);

            return signInResult;
        }

        public async Task Signout()
        {
            await localStorage.RemoveItemAsync("authToken");
            ((CustomAuthenticationStateProvider)authenticationStateProvider).MarkUserAsSignedOut();
        }
    }
}
