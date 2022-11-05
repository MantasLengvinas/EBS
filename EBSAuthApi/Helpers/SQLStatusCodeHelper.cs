using System;
using EBSAuthApi.Constants;

namespace EBSAuthApi.Helpers
{
    public static class SQLStatusCodeHelper
    { 

        public static string HandleStatusCode(int statusCode, string procedure)
        {
            switch (procedure)
            {
                case "login":
                    return HandleLoginStatusCode(statusCode);
                default:
                    return "Could not find procedure";
            }
        }

        private static string HandleLoginStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 1:
                    return SQLErrorConstants.ValueIsNull;
                case 2:
                    return SQLErrorConstants.UserNotFound;
                case -1:
                    return SQLErrorConstants.PasswordIsIncorrect;
                default:
                    return "Unhandled exception occured";
            }
        }
    }
}

