using OpenQA.Selenium;

namespace PajacykRandomizer
{
    class PajacykPage
    {
        private readonly IWebDriver firefoxDriver;
        public const string mainUrl = "https://www.pajacyk.pl";

        public PajacykPage(IWebDriver firefoxDriver) =>
            this.firefoxDriver = firefoxDriver;

        public IWebElement GetDeactivatedBellyButton() =>
            firefoxDriver.FindElement(By.ClassName("paj-click"));

        public IWebElement GetActivatedBellyButton() =>
            firefoxDriver.FindElement(By.ClassName("paj-click2"));
    }
}
