using System;
namespace EBSApp.Options
{
    public class ApiClientOptions
    {
        public const string Position = "API:BaseUrls";
        public string EBSBaseUrl { get; set; }
        public string EBSProdBaseUrl { get; set; }
        public string AuthBaseUrl { get; set; }
        public string AuthProdBaseUrl { get; set; }
    }
}

