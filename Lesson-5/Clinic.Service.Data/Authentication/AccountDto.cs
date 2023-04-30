namespace Clinic.Service.Data.Authentication;

/// <summary>
/// Account entity
/// </summary>
public class AccountDto
{
    /// <summary>
    /// Id
    /// </summary>
    public int AccountId { get; set; }
    /// <summary>
    /// Email
    /// </summary>
    public string EMail { get; set; }
    /// <summary>
    /// Locked
    /// </summary>
    public bool Locked { get; set; }
    /// <summary>
    /// FirstName
    /// </summary>
    public string FirstName { get; set; }
    /// <summary>
    /// LastName
    /// </summary>
    public string LastName { get; set; }
    /// <summary>
    /// SecondName
    /// </summary>
    public string SecondName { get; set; }
}