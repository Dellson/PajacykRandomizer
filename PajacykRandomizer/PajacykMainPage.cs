using System;
using System.Threading;
using MathNet.Numerics.Distributions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using PajacykRandomizer.Pages;
using MathHelper = PajacykRandomizer.Helpers.MathHelper;

namespace PajacykRandomizer
{
    [TestFixture]
    public class PajacykMainPage
    {
        private FirefoxOptions options;

        [SetUp]
        public void SetUp()
        {
            options = new FirefoxOptions();
            options.AddArguments("--headless");
        }

        //[Test]
        public void ClickBellyButton(int loopCount)
        {
            using (IWebDriver driver = new FirefoxDriver(options))
            {
                PajacykPage pajacykPage = new PajacykPage(driver);
                driver.Navigate().GoToUrl(PajacykPage.MainUrl);

                for (int i = 0; i < loopCount; i++)
                {
                    Actions action = new Actions(driver);
                    IWebElement bellyButton = pajacykPage.GetBullyButton();
                    Random randomizer = MathHelper.GenerateRandomWithSeed();
                    double mean = 45000;
                    double stddev = 15000;
                    int delayBeforeReadingClickCount = 3000;
                    int normalizedSample = MathHelper.GetIntNormalizedSample(new Normal(mean, stddev, randomizer));

                    action.MoveToElement(bellyButton).Perform();
                    action.Click(bellyButton).Perform();

                    Thread.Sleep(delayBeforeReadingClickCount);
                    string numberOfClicks = pajacykPage.GetClicksNumber();

                    TestContext.WriteLine($"Normalized sample after click: {normalizedSample}.");
                    TestContext.WriteLine($"Number of clicks: {numberOfClicks}");

                    Thread.Sleep(normalizedSample);
                }
            }
        }

        [Test]
        public void ClickBellyButtonRepeateably() =>
            ClickBellyButton(3);
    }
}