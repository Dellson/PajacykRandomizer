using OpenQA.Selenium;

namespace PajacykRandomizer.Pages
{
    internal class PajacykPage
    {
        public const string MainUrl = "https://www.pajacyk.pl";
        private readonly IWebDriver firefoxDriver;

        public PajacykPage(IWebDriver firefoxDriver) =>
            this.firefoxDriver = firefoxDriver;

        public IWebElement GetBullyButton() =>
            firefoxDriver.FindElement(By.ClassName("pajacyk__clickbox"));

        public string GetClicksNumber() =>
            firefoxDriver.FindElement(By.XPath("//*[@class='pajacyk__thankyou']//span")).Text;
    }
}
