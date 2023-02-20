using Microsoft.AspNetCore.SignalR.Client;
using WorkshopConfTool.Shared.Models;

namespace WorkshopClient.Services
{
    public class SignalRService : IAsyncDisposable
    {
        private HubConnection? _hubConnection;

        public Action<Tuple<string, string>>? ConferenceUpdated;

        public async Task IntiConnection()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7069/conferencesHub")
                .WithAutomaticReconnect(new[] { TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(3) })
                .Build();

            _hubConnection.On("ConferenceUpdated", (ConferenceDetails conf) =>
            {
                ConferenceUpdated?.Invoke(new Tuple<string, string>(conf.Title, conf.City));
            });

            await _hubConnection.StartAsync();
        }


        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
