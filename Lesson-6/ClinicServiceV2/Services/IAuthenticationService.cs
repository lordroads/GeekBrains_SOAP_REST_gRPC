using ClinicServiceV2.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClinicServiceV2.Services;

public interface IAuthenticationService
{
    public AuthenticationResponse Login(AuthenticationRequest authenticationRequest);
    public SessionContext GetSessionInfo(string sessionToken);
}
