using System;
using EBSApp.Models;
using EBSApp.Models.Domain;
using EBSApp.Models.Dtos;
using EBSApp.Services;
using EBSApp.Services.General;
using Microsoft.AspNetCore.Components;

namespace EBSApp.Shared.Components
{
    public partial class ActionBox : ComponentBase
    {
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        UserStore UserStore { get; set; }
        [Inject]
        IUsageService UsageService { get; set; }
        [Inject]
        ITariffService TariffService { get; set; }
        [Parameter]
        public int ID { get; set; }
        [Parameter]
        public int ProviderID { get; set; }
        [Parameter]
        public string? Address { get; set; }
        [Parameter]
        public string? Provider { get; set; }
        [Parameter]
        public string? Business { get; set; } = "False";

        List<Usage> Usages { get; set; } = new();
        decimal PaymentSum { get; set; }

        // New usage record params
        decimal CurrentTariff { get; set; }
        decimal NewSum { get; set; }
        decimal NewUsage { get; set; }
        int TariffId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ApiResponse<GetUsageResponseDto> res = await UsageService.GetUnpaidUsages(ID);
            if (res.IsSuccess)
            {
                Usages = res.Data.Usages.OrderBy(x => x.OnDate).ToList();

            }

            ApiResponse<List<GetTariffResponseDto>> tariffs = await TariffService.GetLatestTariff(ProviderID);

            if (tariffs.IsSuccess)
            {
                CurrentTariff = tariffs.Data.Where(x => x.IsBussiness == false).Select(x => x.Rate).FirstOrDefault();
                TariffId = tariffs.Data.Where(x => x.IsBussiness == false).Select(x => x.TariffId).FirstOrDefault();
            }
        }

        void UsageChanged(ChangeEventArgs e)
        {
            if(e.Value != null)
            {
                if(Decimal.TryParse(e.Value.ToString(), out decimal usage))
                {
                    NewSum = CurrentTariff * usage;
                    NewUsage = usage;
                }
            }
        }

        async Task AddNewUsage()
        {
            UsageDto data = new()
            {
                ElectricityDue = NewUsage,
                OnDate = DateTime.Now.Date,
                AddressId = ID,
                PaidTariff = TariffId,
                IsPaid = false
            };

            if(data.ElectricityDue > 0)
            {
                await UsageService.NewUsage(data);

                Navigation.NavigateTo(Navigation.Uri, true);
            }

        }

        void SelectToPay(int id, ChangeEventArgs e)
        {
            Usages.Find(x => x.UsageId == id).ToPay = bool.Parse(e.Value.ToString());
            PaymentSum = Usages.Where(x => x.ToPay == true).Sum(x => x.AmountDue);
        }

        void PayUsages()
        {
            Usages.ForEach(async x => {
                if (x.ToPay)
                    if(Decimal.Parse(UserStore.Balance) > x.AmountDue)
                        await UsageService.PayUsage(x.UsageId);
            });

            Navigation.NavigateTo(Navigation.Uri, true);
        }
    }
}

