using RESTfulODataService.Services.RegularExpressions.Singleton;

namespace RESTfulODataService.Services.RegularExpressions
{
    public class RegexUtilizer : IRegexUtilizer
    {
        private readonly IRegexSingleton regexSingleton;

        public RegexUtilizer(IRegexSingleton regexSingleton)
        {
            this.regexSingleton = regexSingleton;
        }

        public string RemoveWhiteSpaces(string value)
        {
            return regexSingleton
                .Create(@"\s+")
                .Replace(value, "");
        }

        #region String Splitting

        public string[] SplitOnCommasAndRemoveWhiteSpace(string value)
        {
            return regexSingleton
                .Create(@"\s*,\s*")
                .Split(value);
        }

        public string[] SplitOnSpaceFollowedByDigit(string value)
        {
            return regexSingleton
                .Create("(?= -?\\d+(?:\\.\\d+)?)")
                .Split(value);
        }

        public string[] SplitOnApostrophes(string value)
        {
            return regexSingleton
                .Create(@"[\']")
                .Split(value);
        }
    
        public string[] SplitOnWhiteSpacesOutsideApostrophes(string value)
        {
            return regexSingleton
                .Create(@"(?<=^[^']*(?:'[^']*'[^']*)*) (?=(?:[^']*'[^']*')*[^']*$)")
                .Split(value);
        }

        public string[] SplitOnAndOrOrOutsideApostrophes(string value)
        {
            return regexSingleton
                .Create("(?<=^[^']*(?:'[^']*'[^']*)*)( and )|( or )(?=(?:[^']*'[^']*')*[^']*$)")
                .Split(value);
        }

        public string[] SplitOnWords(string value)
        {
            return regexSingleton
                .Create(@"[^\w]")
                .Split(value);
        }

        public string[] SplitOnWhiteSpace(string value)
        {
            return regexSingleton
                .Create(@"[\s]")
                .Split(value);
        }

        public string[] SplitOnFullStop(string value)
        {
            return regexSingleton
                .Create(".")
                .Split(value);
        }

        #endregion

        #region String Matching

        public bool IsMatchingHexadecimalFormat(string value)
        {
            if (value == null)
            {
                return false;
            }

            return regexSingleton
                .Create(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
                .IsMatch(value);
        }

        public bool IsMatchingContainsLetter(string value)
        {
            if (value == null)
            {
                return false;
            }

            return regexSingleton
                .Create(@"[a-zA-Z]+")
                .IsMatch(value);
        }

        public bool IsMatchingUrlFormat(string value)
        {
            if (value == null)
            {
                return false;
            }

            return regexSingleton
                .Create("(ftp|http|https):\\/\\/(\\w+:{0,1}\\w*@)?(\\S+)(:[0-9]+)?(\\/|\\/([\\w#!:.?+=&%@!\\-\\/]))?")
                .IsMatch(value);
        }

        public bool IsMatchingPhoneNumberFormat(string value)
        {
            if (value == null)
            {
                return false;
            }

            return regexSingleton
                .Create(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$")
                .IsMatch(value);
        }

        public bool IsMatchingEmailFormat(string value)
        {
            if (value == null)
            {
                return false;
            }

            return regexSingleton
                .Create(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")
                .IsMatch(value);
        }

        public bool IsMatchingZipCodeFormat(string value)
        {
            if (value == null)
            {
                return false;
            }

            return regexSingleton
                .Create(@"^\d{5}(?:[-\s]\d{4})?$")
                .IsMatch(value);
        }

        public bool IsMatchingFourDigits(string value)
        {
            if (value == null)
            {
                return false;
            }

            return regexSingleton
                .Create(@"^[0-9]{4}$")
                .IsMatch(value);
        }

        #endregion String Matching
    }
}
