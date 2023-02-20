using Microsoft.JSInterop;
using WorkshopClient.Utils;

namespace WorkshopClient.Services
{
    public class NotificationService : JSModule
    {
        public NotificationService(IJSRuntime js) 
            : base(js, "./js/notification.js")
        {
        }

        public async Task ShowNotificationAsync(string title, string message)
        {
            await InvokeVoidAsync("showConferenceNotification", title, message);
        }
    }
}
