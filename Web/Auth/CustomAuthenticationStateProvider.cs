using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Runtime.InteropServices;
using System.Security.Claims;
namespace Web.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly ProtectedLocalStorage _localStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        
        public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage, ProtectedLocalStorage localStorage)
        {
            _sessionStorage = sessionStorage;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, userSession.Id),
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Email, userSession.Email),
                    new Claim(ClaimTypes.Role, userSession.Role),

                }, "CustomAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }

            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task UpdateAuthenticationStateAsync(UserSession userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if(userSession != null)
            {
                await _sessionStorage.SetAsync("UserSession", userSession);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, userSession.Id),
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Email, userSession.Email),
                    new Claim(ClaimTypes.Role, userSession.Role)
                }));
            }
            else
            {
                await _sessionStorage.DeleteAsync("UserSession");
                claimsPrincipal = _anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        //// Регистрация
        //public override async Task<AuthenticationState> GetRegistrationStateAsync()
        //{
        //    try
        //    {
        //        var userLocalStorageResult = await _localStorage.GetAsync<UserSession>("UserLocal");
        //        var userLocal = userLocalStorageResult.Success ? userLocalStorageResult.Value : null;

        //        // _localStorage.SetAsync("UserLocal", "CfDJ8NXnA1Ch3kFGhP7RPF-GZWzCqyOTQAiYdEX4o2FwAhOxEsyQq5enZk0Fk_cTSvX-tNM0bk4FpkkgPD9e4hL42d0EvPZ7aoWGnX22X_GaMqj7kcAtclJhDxx_HsAkKqpmHBDVSp_ZFup1ijip8zCm0H7_VdTWwYIKLViS34bbHQU9ksq9HlFX5OZJ1Bi37TDsOwtbVXWp21AFL864P4B-QlwR4R3-WDDy-EWeq7i-gdtpI33rjvNTZnyi3sj0Rq2nlI42Cr0r7q2ulLZO_0Mkx4vYCqmHZ7j4IVclbfL71vJCxsh0f1UieJ_c9k4OkGGiQ3I18h29C3SgeMVsiiCccDEazgW59n7zMZSHoi0i7tRabBh3pQvxtyC1-2dBHQLYhcHZGPBR2-N2X4gACJVsi4k");

        //        if (userLocal == null)
        //        {
        //            return await Task.FromResult(new AuthenticationState(_anonymous));
        //        }

        //        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Sid, userLocal.Id),
        //            new Claim(ClaimTypes.Name, userLocal.UserName),
        //            new Claim(ClaimTypes.Email, userLocal.Email),
        //            new Claim(ClaimTypes.Role, userLocal.Role)
        //        }, "CustomAuth"));

        //        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        //    }

        //    catch
        //    {
        //        return await Task.FromResult(new AuthenticationState(_anonymous));
        //    }
        //}

        //public async Task UpdateRegistrationStateAsync(UserSession userSession, UserLocal userLocal)
        //{
        //    ClaimsPrincipal claimsPrincipal;

        //    if (userLocal != null)
        //    {
        //        await _localStorage.SetAsync("UserLocal", userLocal);
        //        claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Sid, userLocal.Id),
        //            new Claim(ClaimTypes.Name, userLocal.FIO),
        //            new Claim(ClaimTypes.Email, userLocal.Email),
        //            new Claim(ClaimTypes.Role, userLocal.Role)
        //        }));
        //    }
        //    else
        //    {
        //        await _sessionStorage.DeleteAsync("UserLocal");
        //        claimsPrincipal = _anonymous;
        //    }
        //    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        //}
    }
}

