namespace Clinic.Service.Data.Authentication;

public class SessionContext
{
    public int SessionId { get; set; }

    public string SessionToken { get; set; }

    public AccountDto Account { get; set; }


}
