namespace RESTfulODataService.Services.Validation.ODataElements.Singleton
{
    public interface IODataElementsValidatorSingleton
    {
        ODataElementsValidator Create(string oDataQueryValue);
    }
}
