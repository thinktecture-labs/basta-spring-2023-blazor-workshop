using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using WorkshopConfTool.Shared.Models;

namespace WorkshopClient.Pages
{
    public partial class ConferenceDetail
    {

        [Inject] private HttpClient _httpClient { get; set; } = default!;
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;
        [Parameter] public Guid Id { get; set; } = default!;

        private EditForm? _editForm;
        private bool _isLoading = true;
        private ConferenceDetails? _conference;

        protected override async Task OnInitializedAsync()
        {
            _conference = await _httpClient.GetFromJsonAsync<ConferenceDetails>($"api/v1/conferences/{Id}");
            _isLoading = false;
            await base.OnInitializedAsync();
        }

        private async Task SaveConference()
        {
            if (_editForm?.EditContext is not null && _editForm.EditContext.Validate())
            {
                await _httpClient.PutAsJsonAsync($"api/v1/conferences/{Id}", _conference);
                _navigationManager.NavigateTo("/conferences");
            }
        }

        private void Cancel()
        {
            _navigationManager.NavigateTo("/conferences");
        }
    }
}