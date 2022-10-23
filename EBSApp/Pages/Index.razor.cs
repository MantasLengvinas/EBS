using System;
using EBSApp.Models;
using Microsoft.AspNetCore.Components;
using Serilog;

namespace EBSApp.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public TokenStore TokenStore { get; set; }

        protected override async Task OnInitializedAsync()
        {
            
        }
    }
}

