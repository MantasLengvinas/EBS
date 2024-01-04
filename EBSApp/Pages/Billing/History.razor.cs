using System;
using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Services;
using Microsoft.AspNetCore.Components;

namespace EBSApp.Pages.Billing
{
    public partial class History : ComponentBase
    {
        [Inject]
        IAddressService AddressService { get; set; }
        [Inject]
        UserStore UserStore { get; set; }

        List<GetAddressResponseDto> Addresses { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            ApiResponse<List<GetAddressResponseDto>> res = await AddressService.GetUserAddresses(UserStore.UserId);

            if (res.IsSuccess)
                Addresses = res.Data;
        }
    }
}

