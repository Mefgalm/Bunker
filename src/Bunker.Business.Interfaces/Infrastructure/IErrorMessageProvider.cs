namespace Bunker.Business.Interfaces.Infrastructure
{
    public interface IErrorMessageProvider
    {
        string CompanyNotFound                   { get; }
        string EmailOrPasswordIsIncorrect        { get; }
        string ChallangeNotFound                 { get; }
        string TaskNotFound                      { get; }
        string PlayerAlreadyHasTeamInThisCompany { get; }
        string TeamNotFound                      { get; }
        string ChallangeAlreadyAccepted          { get; }
        string EmailAlreadyRegistered            { get; }
        string PlayerNotFound                    { get; }
    }
}