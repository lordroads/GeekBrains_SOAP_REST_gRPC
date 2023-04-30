namespace Clinic.Service.Data.Authentication;

public class AuthenticationResponse
{
    public AuthenticationStatus Status { get; set; }

    public SessionContext SessionContext { get; set; }
}
