using System;
using EBSApp.Models;
using Microsoft.AspNetCore.Components;

namespace EBSApp.Pages.ClientView
{
    public partial class Profile : ComponentBase
    {
        [Inject]
        UserStore UserStore { get; set; }
    }
}

