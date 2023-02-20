using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using WorkshopConfTool.Shared.Models;

namespace WorkshopClient.Pages
{
    [SupportedOSPlatform("Browser")]
    public partial class ConferenceDetail
    {

        [Inject] private HttpClient _httpClient { get; set; } = default!;
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;
        [Parameter] public Guid Id { get; set; } = default!;

        private bool _touched;
        private EditForm? _editForm;
        private bool _isLoading = true;
        private ConferenceDetails? _conference;

        protected override async Task OnInitializedAsync()
        {
            await JSHost.ImportAsync("ConferenceDetail",
            "../Pages/ConferenceDetail.razor.js");

            _conference = await _httpClient.GetFromJsonAsync<ConferenceDetails>($"api/v1/conferences/{Id}");
            _isLoading = false;
            await base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (_editForm?.EditContext is not null)
                {
                    _editForm.EditContext.OnFieldChanged += (sender, obj) =>
                    {
                        _touched = true;
                    };
                }
            }
            base.OnAfterRender(firstRender);
        }

        private async Task SaveConference()
        {
            if (_editForm?.EditContext is not null && _editForm.EditContext.Validate())
            {
                await _httpClient.PutAsJsonAsync($"api/v1/conferences/{Id}", _conference);
                _navigationManager.NavigateTo("/conferences");
            }
        }

        private async Task OnBeforeNavigation(LocationChangingContext context)
        {
            if (_touched && !Confirm("Wollen sie wirklich ohne speichern die Seite verlassen?"))
            {
                context.PreventNavigation();
            }
        }

        private void Cancel()
        {
            _navigationManager.NavigateTo("/conferences");
        }

        [JSImport("confirmMessage", "ConferenceDetail")]
        internal static partial bool Confirm(string message);
    }
}