using System;
using Microsoft.AspNetCore.Components;

namespace EBSApp.Shared.Components
{
    public partial class ActionBox : ComponentBase
    {
        [Parameter]
        public int ID { get; set; }
        [Parameter]
        public string? Address { get; set; }
        [Parameter]
        public string? Provider { get; set; }
        [Parameter]
        public string? Business { get; set; } = "False";

    }
}

