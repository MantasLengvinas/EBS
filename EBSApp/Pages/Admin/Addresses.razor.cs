using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EBSApp.Pages.Admin
{
    public class AddressesBase : ComponentBase
    {
        /*
         * Buvo atskirta address puslapio logika į atskirą failą. Tai pagerina kodo skaitomumą ir pagerina testuojamumą.
         * Taip pat buvo patvarkytos klaidos ir suteikiama vartotojui daugiau informatyvumo klaidos atveju.
         */

        [Inject]
        private IAddressService AddressService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private IJSRuntime JS { get; set; }

        public List<GetAddressResponseDto>? Addresses { get; set; } // Buvo pervadintas adresų sąrašas tiesiog į Addresses.

        /*
         * Komentaras: Taip pat, GetAddressResponseDto -> čia kintamojo tipas turėtų būti atspindintis jo funkciją domene - t.y., tai yra Adresas. Dabar jis atspindi jo techninę funkciją - kad tai yra DTO.
         * 
         * Taip, DTO pagrindinė paskirtis yra "pernešti" duomenis tarp skirtingų sistemos lygių ar servisų.
         * Teoriškai reikėtų duomenis reikėtų "sumappinti" į domeno tipo klasę, tačiau tai nėra taisyklė, o tik patarimas ar gairė (guideline).
         * Perdėtinis duomenų "mappinimas" mažina sistemos efektyvumą ir duomenų procesavimas gali užtrukti gerokai ilgiau.
         */

        protected override async Task OnInitializedAsync()
        {
            await LoadAddressesAsync();
        }

        private async Task LoadAddressesAsync()
        {
            ApiResponse<List<GetAddressResponseDto>> addressesResponse = await AddressService.GetAllAddressesAsync();

            if (!addressesResponse.IsSuccess)
            {
                await JS.InvokeAsync<bool>("alert", "Duomenų gauti nepavyko!");
                return;
            }

            Addresses = addressesResponse.Data;
        }

        public async Task DeleteAddressAsync(int id)
        {
            bool confirmed = await JS.InvokeAsync<bool>("confirm", "Ar tikrai norite ištrinti?");

            if (!confirmed)
                return;

            bool deleteWasSuccessful = await AddressService.DeleteAddress(id);

            if (!deleteWasSuccessful)
            {
                await JS.InvokeAsync<bool>("alert", "Adreso ištrinti nepavyko");
                return;
            }

            await LoadAddressesAsync(); // Vietoj viso puslapio perkrovimo, dabar yra atnaujinama tik adresų informacija. Taip yra sutaupoma šiek tiek resursų (naršyklei nereikia atsisiųsti ir užkrauti visų puslapio elementų).
        }
    }
}
