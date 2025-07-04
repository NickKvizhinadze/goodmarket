namespace GoodMarket.Identity.Api.Endpoints;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    
    public static class Account
    {
        private const string Base = $"{ApiBase}/account";
        
        public const string Register = $"{Base}/register";
        
    }
}