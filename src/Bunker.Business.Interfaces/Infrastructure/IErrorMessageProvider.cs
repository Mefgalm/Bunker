namespace Bunker.Business.Interfaces.Infrastructure
{
    public interface IErrorMessageProvider
    {
        string TaskNotFound                      { get; }
        string TeamNotFound                      { get; }
        string AnswerIsWrong                     { get; }
        string PlayerNotFound                    { get; }
        string CompanyNotFound                   { get; }
        string ChallangeNotFound                 { get; }
        string JoinKeyIsInvalid                  { get; }
        string EmailAlreadyRegistered            { get; }
        string PlayerAlreadyAddedToTeam           { get; }
        string ChallangeAlreadyAccepted          { get; }
        string PlayerAlreadyAddedToCompany        { get; }
        string EmailOrPasswordIsIncorrect        { get; }
        string PlayerDoesNotExistInThisTeam      { get; }
        string PlayerDoesNotExistInThisCompany   { get; }
        string PlayerAlreadyHasTeamInThisCompany { get; }
    }
}