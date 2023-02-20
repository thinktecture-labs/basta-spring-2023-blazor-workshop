using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace WorkshopClient.Components
{
    public partial class LoginDisplay
    {
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private SignOutSessionStateManager _signOutSessionStateManager { get; set; }
        private async Task BeginSignOut(MouseEventArgs args)
        {
            await _signOutSessionStateManager.SetSignOutState();
            _navigationManager.NavigateToLogout("authentication/logout");
        }

    }
}