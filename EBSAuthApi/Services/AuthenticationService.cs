using System;
using EBSAuthApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EBSAuthApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EBSAuthApi.Models.Dtos.Requests;
using EBSAuthApi.Models.Dtos.Responses;
using EBSAuthApi.Models.Domain;
using EBSAuthApi.Constants;
using Dapper;
using EBSAuthApi.Data;
using EBSAuthApi.Helpers;
using System.Security.Cryptography;
using System.Reflection.Metadata.Ecma335;
using System.Net;

namespace EBSAuthApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationOptions _options;

        private readonly IJwtGenerator _jwtGenerator;

        private readonly IAuthenticationQueries _authQueries;

        public AuthenticationService(
            IOptions<AuthenticationOptions> options, 
            IJwtGenerator jwtGenerator, 
            IAuthenticationQueries authQueries)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
            _authQueries = authQueries ?? throw new ArgumentNullException(nameof(authQueries));
        }

        //private async Task<(bool, string?, User?)> AuthenticateUserAsync(UserInfo userInfo)
        //{
        //    User user = new();
        //    bool isSuccess = false;
        //    string? errorMessage = null;
        //    List<Claim> userClaims = new();

        //    try
        //    {
        //        if (!userInfo.Completed)
        //        {
        //            userClaims.Add(new Claim(ClaimsConstants.ClientId, userInfo.ClientId.ToString()));
        //            userClaims.Add(new Claim(ClaimsConstants.Email, userInfo.Email));
        //            userClaims.Add(new Claim(ClaimsConstants.Completed, userInfo.Completed.ToString()));
        //        }
        //        else
        //        {
        //            userClaims.Add(new Claim(ClaimsConstants.ClientId, userInfo.ClientId.ToString()));
        //            userClaims.Add(new Claim(ClaimsConstants.Id, userInfo.Id.ToString()));
        //            userClaims.Add(new Claim(ClaimsConstants.Email, userInfo.Email));
        //            userClaims.Add(new Claim(ClaimsConstants.FullName, userInfo.FullName));
        //            userClaims.Add(new Claim(ClaimsConstants.Balance, userInfo.Balance.ToString()));
        //            userClaims.Add(new Claim(ClaimsConstants.Active, userInfo.Active.ToString()));
        //            userClaims.Add(new Claim(ClaimsConstants.Business, userInfo.Business.ToString()));
        //            userClaims.Add(new Claim(ClaimsConstants.Completed, userInfo.Completed.ToString()));
        //        }

        //        isSuccess = true;
        //    }
        //    catch(Exception)
        //    {
        //        errorMessage = "Failed to get user info";
        //        return (isSuccess, errorMessage, null);
        //    }

        //    user.UserClaims = userClaims;
        //    user.UserInfo = userInfo;

        //    return (isSuccess, null, user);
        //}

        //public async Task<AuthResponseDto> LoginUserAsync(UserLogin userLogin, CancellationToken cancelToken)
        //{
        //    AuthResponseDto response = new();

        //    if (userLogin == null)
        //    {
        //        response.ErrorMessage = "No user login information";
        //        return response;
        //    }

        //    if(string.IsNullOrEmpty(userLogin.Email) && string.IsNullOrEmpty(userLogin.Password))
        //    {
        //        response.ErrorMessage = "Missing credentials";
        //        return response;
        //    }

        //    (int passwordReturnValue, string? savedPassword) = await _authQueries.GetPassword(userLogin.Email, cancelToken);

        //    if(passwordReturnValue != 0)
        //    {
        //        response.ErrorMessage = "Failed to authenticate";
        //        return response;
        //    }

        //    if(savedPassword == null)
        //    {
        //        response.ErrorMessage = "Failed to retrieve password";
        //        return response;
        //    }

        //    if(!PasswordHelper.ValidatePassword(userLogin.Password, savedPassword))
        //    {
        //        response.ErrorMessage = "Password is incorrect";
        //        return response;
        //    }

        //    (int returnValue, UserInfo? userInfo) = await _authQueries.LoginUser(userLogin.Email, userLogin.Password, cancelToken);

        //    if(returnValue != 0)
        //    {
        //        response.ErrorMessage = SQLStatusCodeHelper.HandleStatusCode(returnValue, "login");
        //        return response;
        //    }

        //    (bool authSuccess, string? authErrorMessage, User? user) = await AuthenticateUserAsync(userInfo);

        //    if (!authSuccess)
        //    {
        //        response.ErrorMessage = authErrorMessage;
        //        return response;
        //    }

        //    string sessionToken = _jwtGenerator.CreateSessionToken(user, _options.Audience, _options.Issuer, _options.ExpirationTimeInSeconds);

        //    response.SessionToken = sessionToken;
        //    response.IsSuccess = true;

        //    return response;
            
        //}

        //public async Task<AuthResponseDto> RegisterClientAsync(ClientRegister clientRegister, CancellationToken cancelToken)
        //{
        //    AuthResponseDto response = new();

        //    if (clientRegister == null)
        //    {
        //        response.ErrorMessage = "No user login information";
        //        return response;
        //    }

        //    if (string.IsNullOrEmpty(clientRegister.Email) && string.IsNullOrEmpty(clientRegister.Password))
        //    {
        //        response.ErrorMessage = "Missing credentials";
        //        return response;
        //    }

        //    clientRegister.Password = PasswordHelper.HashPassword(clientRegister.Password);

        //    (int returnValue, int id) = await _authQueries.RegisterUser(clientRegister.Email, clientRegister.Password, cancelToken);

        //    if(returnValue != 0)
        //    {
        //        response.ErrorMessage = "Registration failed";
        //        return response;
        //    }

        //    UserInfo userInfo = new();
        //    userInfo.ClientId = id;
        //    userInfo.Email = clientRegister.Email;

        //    (bool authSuccess, string? authErrorMessage, User? user) = await AuthenticateUserAsync(userInfo);

        //    if (!authSuccess)
        //    {
        //        response.ErrorMessage = authErrorMessage;
        //        return response;
        //    }

        //    string sessionToken = _jwtGenerator.CreateSessionToken(user, _options.Audience, _options.Issuer, _options.ExpirationTimeInSeconds);

        //    response.SessionToken = sessionToken;
        //    response.IsSuccess = true;

        //    return response;
        //}

        //public async Task<bool> CompleteRegistrationAsync(CompleteRegistration userInfo, CancellationToken cancelToken)
        //{
        //    if (userInfo == null)
        //        return false;

        //    int returnValue = await _authQueries.CompleteRegistration(userInfo, cancelToken);

        //    if (returnValue != 0)
        //        return false;

        //    return true;
        //}

        public void Test()
        {

        }

        public async Task<AuthResponseDto> LoginUserAsync(IUserCredentials credentials, CancellationToken cancelToken = default)
        {
            if (!ValidateCredentials(credentials, out string errorMessage))
                return CreateAuthenticationResponse(errorMessage);

            string savedPassword = await GetSavedPasswordAsync(credentials, cancelToken);

            if (savedPassword is null)
                return CreateAuthenticationResponse("Failed to get user password");

            if (!PasswordHelper.ValidatePassword(credentials.Password, savedPassword))
                return CreateAuthenticationResponse("Password is incorrect");

            (int returnValue, UserInfo userInfo) = await _authQueries.LoginUser(credentials.Email, cancelToken);

            if (returnValue != 0)
                return CreateAuthenticationResponse(SQLStatusCodeHelper.HandleStatusCode(returnValue, "login"));

            string sessionToken = CreateUserSessionToken(userInfo, ref errorMessage);

            if (sessionToken is null)
                return CreateAuthenticationResponse(errorMessage);

            return CreateAuthenticationResponse(sessionToken, true);
        }

        public async Task<AuthResponseDto> RegisterUserAsync(IUserCredentials credentials, CancellationToken cancelToken = default)
        {
            if (!ValidateCredentials(credentials, out string errorMessage))
                return CreateAuthenticationResponse(errorMessage);

            (int returnValue, int id) = await _authQueries.RegisterUser(credentials.Email, PasswordHelper.HashPassword(credentials.Password), cancelToken);

            if (returnValue != 0)
                return CreateAuthenticationResponse("Failed to register new user");

            string sessionToken = CreateUserSessionToken(new()
            {
                ClientId = id,
                Email = credentials.Email
            }, ref errorMessage);

            if (sessionToken is null)
                return CreateAuthenticationResponse(errorMessage);

            return CreateAuthenticationResponse(sessionToken, true);
        }

        public async Task<bool> CompleteRegistrationAsync(CompleteRegistration userInfo, CancellationToken cancelToken = default)
        {
            if (userInfo == null)
                return false;

            int returnValue = await _authQueries.CompleteRegistration(userInfo, cancelToken);

            if (returnValue != 0)
                return false;

            return true;
        }

        private async Task<string> GetSavedPasswordAsync(IUserCredentials credentials, CancellationToken cancelToken)
        {
            (int returnValue, string savedPassword) = await _authQueries.GetPassword(credentials.Email, cancelToken);

            if (returnValue != 0)
                return null;

            return savedPassword;
        }

        private string CreateUserSessionToken(UserInfo userInfo, ref string errorMessage)
        {
            try
            { 
                User user = new()
                {
                    UserInfo = userInfo,
                    UserClaims = CreateUserClaims(userInfo),
                };

                return _jwtGenerator.CreateSessionToken(user, _options.Audience, _options.Issuer, _options.ExpirationTimeInSeconds);
            }
            catch(Exception ex)
            {
                errorMessage = $"Failed to create session token ({ex.Message})";
                return null;
            }
        }

        private static List<Claim> CreateUserClaims(UserInfo userInfo)
        {
            return new()
            {
                new Claim(ClaimsConstants.ClientId, userInfo.ClientId.ToString()),
                new Claim(ClaimsConstants.Id, userInfo.Id.ToString()),
                new Claim(ClaimsConstants.Email, userInfo.Email),
                new Claim(ClaimsConstants.FullName, userInfo.FullName),
                new Claim(ClaimsConstants.Balance, userInfo.Balance.ToString()),
                new Claim(ClaimsConstants.Active, userInfo.Active.ToString()),
                new Claim(ClaimsConstants.Business, userInfo.Business.ToString()),
                new Claim(ClaimsConstants.Completed, userInfo.Completed.ToString())
            };
        }

        private static bool ValidateCredentials(IUserCredentials credentials, out string errorMessage)
        {
            if (credentials is null || string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
            {
                errorMessage = "Missing credentials";
                return false;
            }

            errorMessage = null;
            return true;
        }

        private static AuthResponseDto CreateAuthenticationResponse(string errorMessage)
        {
            return new AuthResponseDto(errorMessage);
        }

        private static AuthResponseDto CreateAuthenticationResponse(string sessionToken, bool isSuccess)
        {
            return new AuthResponseDto(sessionToken, isSuccess);
        }
    }
}

