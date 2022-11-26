using System;
using EBSApp.Models;
using Microsoft.AspNetCore.Components;
using Serilog;

namespace EBSApp.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        TokenStore TokenStore { get; set; }

        int Address { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            
        }
    }
}

