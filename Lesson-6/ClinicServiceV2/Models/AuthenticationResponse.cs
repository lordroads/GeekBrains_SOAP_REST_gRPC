namespace ClinicServiceV2.Models;

public class AuthenticationResponse
{
    public AuthenticationStatus Status { get; set; }

    public SessionContext SessionContext { get; set; }
}
