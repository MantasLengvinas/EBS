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
        NavigationManager Navigation { get; set; }
        [Inject]
        IProviderService ProviderService { get; set; }
        [Inject]
        IAddressService AddressService { get; set; }

        [Parameter]
        public bool IsOpen { get; set; }
        [Parameter]
        public string UserId { get; set; }

        public void Open() => IsOpen = true;

        public void Close() => IsOpen = false;

        public int ProviderId { get; set; }
        public string Address { get; set; }

        List<GetProviderResponseDto> Providers { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            ApiResponse<List<GetProviderResponseDto>> res = await ProviderService.GetProviders();

            if (res.IsSuccess)
                Providers = res.Data;
        }

        async Task AddNewAddress()
        {
            GetAddressResponseDto address = new()
            {
                FullAddress = Address,
                ProviderId = ProviderId,
                UserId = int.Parse(UserId)
            };

            await AddressService.AddNewAddress(address);
            Navigation.NavigateTo("/");
        }
    }
}

