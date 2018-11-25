using RESTfulODataService.Services.RegularExpressions;
using RESTfulODataService.Services.RegularExpressions.Singleton;
using System.Threading.Tasks;
using Xunit;

namespace RESTfulODataService.Tests.Services.RegularExpressions
{
    public class RegexUtilizerTests
    {
        private RegexUtilizer RegexUtilizer
        {
            get
            {
                IRegexSingleton regexSingleton = new RegexSingleton();

                return new RegexUtilizer(regexSingleton);
            }
        }

        [Fact(DisplayName = "RegexUtilizer_ShouldSplitString_OnApostrophes")]
        public async Task SplitOnApostrophes()
        {
            //Arrange
            var stringToSplit = "this' string' contain's ap'ostrophes";

            //Act
            string[] splitString = RegexUtilizer.SplitOnApostrophes(stringToSplit);

            //Assert
            Assert.Equal("this", splitString[0]);
            Assert.Equal(" string", splitString[1]);
            Assert.Equal(" contain", splitString[2]);
            Assert.Equal("s ap", splitString[3]);
            Assert.Equal("ostrophes", splitString[4]);
        }

        [Fact(DisplayName = "RegexUtilizer_ShouldRemoveWhiteSpace_FromStringContainingWhiteSpace")]
        public async Task RemoveWhiteSpace()
        {
            //Arrange
            var stringToRemoveWhiteSpace = "this string    contains white          space";

            //Act
            string noWhiteSpaceString = RegexUtilizer.RemoveWhiteSpaces(stringToRemoveWhiteSpace);

            //Assert
            Assert.Equal("thisstringcontainswhitespace", noWhiteSpaceString);
        }

        [Fact(DisplayName = "RegexUtilizer_ShouldSplitString_OnSpaceFollowedByDigit")]
        public async Task SplitOnDigit()
        {
            //Arrange
            var stringWithDigits = "this 3 string 12211 has3 digits";

            //Act
            string[] splitString = RegexUtilizer.SplitOnSpaceFollowedByDigit(stringWithDigits);

            //Assert
            Assert.Equal("this", splitString[0]);
            Assert.Equal(" 3 string", splitString[1]);
            Assert.Equal(" 12211 has3 digits", splitString[2]);
        }

        [Fact(DisplayName = "RegexUtilizer_ShouldSplitString_OnSpacesOutsideApostrophes")]
        public async Task SplitOnWhiteSpacesOutsideApostrophes()
        {
            //Arrange
            var stringToSplit = "this' string contains' apostrophes !";

            //Act
            string[] splitString = RegexUtilizer.SplitOnWhiteSpacesOutsideApostrophes(stringToSplit);

            //Assert
            Assert.Equal("this' string contains'", splitString[0]);
            Assert.Equal("apostrophes", splitString[1]);
            Assert.Equal("!", splitString[2]);
        }

        [Fact(DisplayName = "RegexUtilizer_ShouldSplitString_On'and'OutsideApostrophes")]
        public async Task SplitOnAndOutsideApostrophes()
        {
            //Arrange
            var stringToSplit = "startswith(name, 'iskandar') and phrase equals 'you and me'";

            //Act
            string[] splitString = RegexUtilizer.SplitOnAndOrOrOutsideApostrophes(stringToSplit);

            //Assert
            Assert.Equal("startswith(name, 'iskandar')", splitString[0]);
            Assert.Equal(" and ", splitString[1]);
            Assert.Equal("phrase equals 'you and me'", splitString[2]);
        }

        [Fact(DisplayName = "RegexUtilizer_ShouldSplitString_On'or'OutsideApostrophes")]
        public async Task SplitOnOrOutsideApostrophes()
        {
            //Arrange
            var stringToSplit = "startswith(name, 'dog') or phrase equals 'you or me'";

            //Act
            string[] splitString = RegexUtilizer.SplitOnAndOrOrOutsideApostrophes(stringToSplit);

            //Assert
            Assert.Equal("startswith(name, 'dog')", splitString[0]);
            Assert.Equal(" or ", splitString[1]);
            Assert.Equal("phrase equals 'you or me'", splitString[2]);
        }

        [Fact(DisplayName = "RegexUtilizer_ShouldSplitString_OnWords")]
        public async Task SplitonWords()
        {
            //Arrange
            var stringToSplit = "splitting on words 12 .";

            //Act
            string[] splitString = RegexUtilizer.SplitOnWords(stringToSplit);

            //Assert
            Assert.Equal("splitting", splitString[0]);
            Assert.Equal("on", splitString[1]);
            Assert.Equal("words", splitString[2]);
            Assert.Equal("12", splitString[3]);
        }

        [Fact(DisplayName = "RegexUtilizer_ShouldSplitString_OnWhiteSpace")]
        public async Task SplitonWhitespace()
        {
            //Arrange
            var stringToSplit = "splitting on white space .";

            //Act
            string[] splitString = RegexUtilizer.SplitOnWhiteSpace(stringToSplit);

            //Assert
            Assert.Equal("splitting", splitString[0]);
            Assert.Equal("on", splitString[1]);
            Assert.Equal("white", splitString[2]);
            Assert.Equal("space", splitString[3]);
            Assert.Equal(".", splitString[4]);
        }
    }
}
