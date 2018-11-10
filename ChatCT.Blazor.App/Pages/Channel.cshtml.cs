using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;

namespace ChatCT.Blazor.App.Pages
{
    public class ChannelBase : BlazorComponent
    {
        [Parameter]
        protected List<int> Numbers { get; set; } = new List<int> { 1, 2, 3 };

        protected override void OnInit()
        {
            Task.Factory.StartNew(GenerateNumbers, TaskCreationOptions.LongRunning);
        }

        private void GenerateNumbers()
        {
            var random = new Random();
            while (true)
            {
                Numbers.Add(random.Next(100));
                StateHasChanged();
                Thread.Sleep(300);
            }
        }
    }
}
