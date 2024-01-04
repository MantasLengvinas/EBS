using System;
namespace EBSAuthenticationHandler.Defaults
{
    public class EBSAuthenticationDefaults
    {
        public const string AuthenticationScheme = "EBSAuth";
        public const string ApiKeyHeaderName = "X-API-KEY";

        internal const string LoginUrl = "Auth/Login";
        internal const string RegisterUrl = "Auth/Register";
        internal const string CompleteRegistrationUrl = "Auth/Complete";
    }
}

