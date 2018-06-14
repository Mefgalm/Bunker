namespace Bunker.Business.Interfaces.Infrastructure
{
    public interface IErrorMessageProvider
    {
        string CompanyNotFound { get; }
        string EmailOrPasswordIsIncorrect { get; }  
        string ChallangeNotFound { get; }
    }
}