using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;

namespace ChatCT.Blazor.App.Pages
{
    public class IndexBase : BlazorComponent
    {
        [Inject] private IUriHelper UriHelper { get; set; }

        [Parameter] protected string Channel { get; set; }

        [Parameter] protected string UserName { get; set; }

        [Parameter] protected string Token { get; set; }

        protected void OpenChannel()
        {
            UriHelper.NavigateTo($"Channel/{Channel}/{UserName}/{Token}");
        }
    }
}