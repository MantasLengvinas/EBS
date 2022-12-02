using System;
using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Services;
using EBSApp.Shared.Components;
using Microsoft.AspNetCore.Components;

namespace EBSApp.Pages.ClientView
{
    public partial class Profile : ComponentBase
    {
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

