using System;
using Microsoft.AspNetCore.Components;
using Serilog;

namespace EBSApp.Pages
{
    public partial class Index : ComponentBase
    {

        protected override async Task OnInitializedAsync()
        {
            Serilog.Log.Error("Test");
        }
    }
}

