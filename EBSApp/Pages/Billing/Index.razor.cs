using System;
using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Models.Dtos.Responses;
using EBSApp.Services;
using EBSApp.Shared.Components;
using Microsoft.AspNetCore.Components;
using Serilog;

namespace EBSApp.Pages.Billing
{
    public partial class Index : ComponentBase
    {
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        TokenStore TokenStore { get; set; }
        [Inject]
        UserStore UserStore { get; set; }
        [Inject]
        IAddressService AddressService { get; set; }

        AddAddress AddressModal { get; set; }

        List<GetAddressResponseDto> Addresses { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            ApiResponse<List<GetAddressResponseDto>> res = await AddressService.GetUserAddresses(UserStore.UserId);

            if (res.IsSuccess)
                Addresses = res.Data;
        }
    }
}

