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
        public string ChallangeAlreadyAccepted          => "Challange already accepted";
        public string EmailAlreadyRegistered            => "Email already registered";
        public string EmailOrPasswordIsIncorrect        => "Email or password is incorrect";
        public string PlayerAlreadyHasTeamInThisCompany => "Player already has team in this company";

        private static string EntityNotFound(string entityName) => $"{entityName} not found";
    }
}