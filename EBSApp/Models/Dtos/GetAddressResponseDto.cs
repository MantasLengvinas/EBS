using System;
namespace EBSApp.Models.Dtos
{
    public class GetAddressResponseDto
    {
        public int AddressId { get; set; }
        public string? FullAddress { get; set; }
        public int UserId { get; set; }
        public string ProviderName { get; set; }
        public int ProviderId { get; set; }
        public bool Active { get; set; }
    }
}

