namespace RESTfulODataService.Models.RESTful
{
    public class CustomHrefHelperModel
    {
        public CustomHrefType HrefType { get; set; }

        public string CustomHref { get; set; }
    }

    public enum CustomHrefType
    {
        Relative,
        Absolute,
        Ignore
    }
}
