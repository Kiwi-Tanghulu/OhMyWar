using System.Threading.Tasks;
using Unity.Services.Authentication;

public static class Authenticator
{
    public static AuthState AuthState { get; private set; } = AuthState.NonAuthenticated;

    public static async Task<AuthState> DoAuth(AuthProvider provider = AuthProvider.Anonymously, int retries = 5)
    {
        if(AuthState == AuthState.NonAuthenticated)
            await AuthenticateAsync(provider, retries);

        return AuthState;
    }

    private static async Task AuthenticateAsync(AuthProvider provider, int retries)
    {
        AuthState = AuthState.Authenticating;

        switch(provider)
        {
            case AuthProvider.Anonymously:
                await SignInAnnonymouslyAsync(retries);
                break;
        }
    }

    private static async Task SignInAnnonymouslyAsync(int retries)
    {
        int tries = 0;
        while(AuthState == AuthState.Authenticating && tries < retries)
        {
            try
            {
                IAuthenticationService auth = AuthenticationService.Instance;
                await auth.SignInAnonymouslyAsync();
                if(auth.IsSignedIn && auth.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }
            }
            catch
            {
                AuthState = AuthState.Error;
            }
            tries++;
            await Task.Delay(1000);
        }
    }
}
