using ClinicServiceNamespace;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using static ClinicServiceNamespace.AuthenticateService;

namespace ClinicServiceV2.Services;

[Authorize]
public class AuthService : AuthenticateServiceBase
{
    private readonly IAuthenticationService _authenticationService;
    public AuthService(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    public override Task<AuthenticationResponse> Login(AuthenticationRequest request, ServerCallContext context)
    {
        Models.AuthenticationResponse response = _authenticationService.Login(new Models.AuthenticationRequest
        {
            Login = request.UserName,
            Password = request.Password
        });

        if (response.Status == Models.AuthenticationStatus.Success)
        {
            context.ResponseTrailers.Add("X-Session-Token", response.SessionContext.SessionToken);
        }

        return Task.FromResult(new AuthenticationResponse
        {
            Status = (int)response.Status,
            SessionContext = new SessionContext
            {
                SessionId = response.SessionContext.SessionId,
                SessionToken = response.SessionContext.SessionToken,
                Account = new AccountDto
                {
                    AccountId = response.SessionContext.Account.AccountId,
                    EMail = response.SessionContext.Account.EMail,
                    FirstName= response.SessionContext.Account.FirstName,
                    LastName = response.SessionContext.Account.LastName,
                    SecondName = response.SessionContext.Account.SecondName,
                    Locked = response.SessionContext.Account.Locked
                }
            }
        });
    }

    public override Task<GetSessionResponse> GetSession(GetSessionRequest request, ServerCallContext context)
    {
        var authorizationHeader = context.RequestHeaders.FirstOrDefault(header => header.Key == "Authorization");
        if (AuthenticationHeaderValue.TryParse(authorizationHeader.Value, out var headerValue))
        {
            var sheme = headerValue.Scheme;
            var sessionToken = headerValue.Parameter;
            if (string.IsNullOrEmpty(sessionToken))
            {
                return Task.FromResult(new GetSessionResponse());
            }

            Models.SessionContext sessionContext = _authenticationService.GetSessionInfo(sessionToken);

            if (sessionContext is null)
            {
                return Task.FromResult(new GetSessionResponse());
            }

            return Task.FromResult(new GetSessionResponse
            {
                SessionContext = new SessionContext
                {
                    SessionId = sessionContext.SessionId,
                    SessionToken = sessionContext.SessionToken,
                    Account = new AccountDto
                    {
                        AccountId = sessionContext.Account.AccountId,
                        EMail = sessionContext.Account.EMail,
                        FirstName = sessionContext.Account.FirstName,
                        LastName = sessionContext.Account.LastName,
                        SecondName = sessionContext.Account.SecondName,
                        Locked = sessionContext.Account.Locked
                    }
                }
            });
        }

        return Task.FromResult(new GetSessionResponse());
    }
}
