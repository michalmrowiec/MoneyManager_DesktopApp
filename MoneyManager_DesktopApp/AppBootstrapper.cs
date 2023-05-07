using MoneyManager_DesktopApp.Services;
using Splat;

namespace MoneyManager_DesktopApp;

public class AppBootstrapper
{
    public AppBootstrapper() 
    { 
        Locator.CurrentMutable.RegisterConstant(new JwtTokenService(), typeof(JwtTokenService));
        Locator.CurrentMutable.RegisterConstant(new HttpClientService(), typeof(IHttpClientService));
        // Other registrations go here...
    } 
}