namespace RESTfulODataService.Services.RegularExpressions
{
    public interface IRegexUtilizer
    {
        string RemoveWhiteSpaces(string value);

        string[] SplitOnApostrophes(string value);

        string[] SplitOnWords(string value);

        string[] SplitOnWhiteSpacesOutsideApostrophes(string value);

        string[] SplitOnWhiteSpace(string value);

        string[] SplitOnSpaceFollowedByDigit(string value);

        string[] SplitOnAndOrOrOutsideApostrophes(string value);

        string[] SplitOnCommasAndRemoveWhiteSpace(string value);

        string[] SplitOnFullStop(string value);

        bool IsMatchingHexadecimalFormat(string value);

        bool IsMatchingContainsLetter(string value);

        bool IsMatchingUrlFormat(string value);

        bool IsMatchingPhoneNumberFormat(string value);

        bool IsMatchingEmailFormat(string value);

        bool IsMatchingZipCodeFormat(string value);

        bool IsMatchingFourDigits(string value);
    }
}
