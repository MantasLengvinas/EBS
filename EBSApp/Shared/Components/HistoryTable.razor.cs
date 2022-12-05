using System;
using EBSApp.Models;
using EBSApp.Models.Domain;
using EBSApp.Models.Dtos;
using EBSApp.Services;
using Microsoft.AspNetCore.Components;

namespace EBSApp.Shared.Components
{
    public partial class HistoryTable : ComponentBase
    {
        [Inject]
        IUsageService UsageService { get; set; }
        [Parameter]
        public int ID { get; set; }
        [Parameter]
        public string Address { get; set; }

        List<Usage> Usages { get; set; } = new();
        decimal TotalSum { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ApiResponse<GetUsageResponseDto> res = await UsageService.GetUsageHistory(ID);
            if (res.IsSuccess)
            {
                Usages = res.Data.Usages.OrderBy(x => x.ElectricityDue).ToList();
                TotalSum = res.Data.PaymentSum;
            }
        }

    }
}

