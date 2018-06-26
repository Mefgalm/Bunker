using Bunker.Business.Interfaces.Infrastructure;

namespace Bunker.Business.Infrastructure
{
    public class RuntimeErrorMessageProvider : IErrorMessageProvider
    {
        public string CompanyNotFound                   => EntityNotFound("Company");
        public string ChallangeNotFound                 => EntityNotFound("Challange");
        public string TaskNotFound                      => EntityNotFound("Task");
        public string TeamNotFound                      => EntityNotFound("Team");
        public string PlayerNotFound                    => EntityNotFound("Player");
        public string JoinKeyIsInvalid                  => "Join key is invalid";
        public string AnswerIsWrong                     => "Answer is wrong";
        public string PlayerAlreadyAddedToTeam          => "Player already added to this team";
        public string ChallangeAlreadyAccepted          => "Challange already accepted";
        public string PlayerAlreadyAddedToCompany       => "Player already added to this company";
        public string EmailAlreadyRegistered            => "Email already registered";
        public string EmailOrPasswordIsIncorrect        => "Email or password is incorrect";
        public string PlayerDoesNotExistInThisTeam      => "Player doesn't exist in team";
        public string PlayerDoesNotExistInThisCompany   => "Player doesn't exist in company";
        public string PlayerAlreadyHasTeamInThisCompany => "Player already has team in this company";

        private static string EntityNotFound(string entityName) => $"{entityName} not found";
    }
}