using System;
using System.Security.Claims;
using EBSApp.Models.Authentication;
using EBSAuthenticationHandler.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace EBSApp.Pages.ClientView
{
    public partial class ClientSetup : ComponentBase
    {
        [Inject]
        IUserAuthService AuthService { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        AuthenticationStateProvider AuthStateProvider { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        public UserInfo UserInfo { get; set; } = new();

        public async Task CompleteRegistration()
        {
            AuthenticationState authState = await AuthStateProvider.GetAuthenticationStateAsync();

            Claim? id = authState.User.Claims.FirstOrDefault(x => x.Type == "clientId");

            if (id == null)
                return;
            if (Name == null || Surname == null)
                return;

            UserInfo.Id = int.Parse(id.Value);
            UserInfo.FullName = (Name + " " + Surname).Trim();

            if (await AuthService.CompleteUserRegistration(UserInfo))
            {
                Navigation.NavigateTo("./logout", true);
            }
        }
    }
}

