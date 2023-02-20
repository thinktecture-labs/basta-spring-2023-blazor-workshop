using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using WorkshopClient.Services;
using WorkshopConfTool.Shared.Models;

namespace WorkshopClient.Pages
{
    public partial class Conferences
    {
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;
        [Inject] private HttpClient _httpClient { get; set; } = default!;
        [Inject] private SignalRService _signalRService { get; set; } = default!;
        [Inject] private NotificationService _notificationService { get; set; } = default!;

        private bool _isLoading = true;
        private List<ConferenceOverview> _conferences = new();

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("Conferences | InitAsync");
            _signalRService.ConferenceUpdated += async (updateInfo) =>
            {
                await _notificationService.ShowNotificationAsync("Eine Konferenz wurde aktuallisiert", updateInfo.Item1);
            };

            await _signalRService.IntiConnection();
            _conferences = await _httpClient.GetFromJsonAsync<List<ConferenceOverview>>("api/v1/conferences") ?? new();
            _isLoading = false;
            await base.OnInitializedAsync();
        }

        private void OpenDetails(Guid id)
        {
            _navigationManager.NavigateTo($"/conferences/{id}");
        }
    }
}