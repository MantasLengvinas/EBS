﻿namespace EBSApi.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string? FullAddress { get; set; }
        public int UserId { get; set; }
        public int ProviderId { get; set; }
    }
}