using Splat;

namespace MoneyManager_DesktopApp;

public class AppBootstrapper
{
    public AppBootstrapper() 
    { 
        Locator.CurrentMutable.RegisterConstant(new JwtTokenService(), typeof(JwtTokenService));
        // Other registrations go here...
    } 
}