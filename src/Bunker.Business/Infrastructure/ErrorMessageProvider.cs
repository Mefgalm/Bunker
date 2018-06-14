using Bunker.Business.Interfaces.Infrastructure;

namespace Bunker.Business.Infrastructure
{
    public class ErrorMessageProvider : IErrorMessageProvider
    {
        public string CompanyNotFound => "Company not found";
        public string EmailOrPasswordIsIncorrect => "Email or password is incorrect";
        public string ChallangeNotFound => "Challange not found";
    }
}