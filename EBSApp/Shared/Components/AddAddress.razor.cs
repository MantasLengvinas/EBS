using System;
using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Services;
using Microsoft.AspNetCore.Components;

namespace EBSApp.Shared.Components
{
    public partial class AddAddress : ComponentBase
    {
        [Inject]
        IProviderService ProviderService { get; set; }

        [Parameter]
        public bool IsOpen { get; set; }

        public void Open() => IsOpen = true;

        public void Close() => IsOpen = false;

        List<GetProviderResponseDto> Providers { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            ApiResponse<List<GetProviderResponseDto>> res = await ProviderService.GetProviders();

            if (res.IsSuccess)
                Providers = res.Data;
        }
    }
}

