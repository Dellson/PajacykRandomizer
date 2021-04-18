using System;
using System.Threading;
using MathNet.Numerics.Distributions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace PajacykRandomizer
{
    [TestFixture]
    public class PajacykMainPage
    {
        [Test]
        public void ClickBellyButton(int loopCount)
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArguments("--headless");

            using (IWebDriver driver = new FirefoxDriver(options))
            {
                PajacykPage pajacykPage = new PajacykPage(driver);
                driver.Navigate().GoToUrl(PajacykPage.MainUrl);

                for (int i = 0; i < loopCount; i++)
                {
                    Actions action = new Actions(driver);
                    IWebElement bellyButton = pajacykPage.GetBullyButton();
                    Random randomizer = GenerateRandomWithSeed();
                    double mean = 45000;
                    double stddev = 13000;
                    int delayBeforeReadingClickCount = 3000;
                    int normalizedSample = GetIntNormalizedSample(new Normal(mean, stddev, randomizer));

                    action.MoveToElement(bellyButton).Perform();
                    action.Click(bellyButton).Perform();

                    Thread.Sleep(delayBeforeReadingClickCount);
                    string numberOfClicks = pajacykPage.GetClicksNumber();

                    TestContext.WriteLine($"Normalized sample after click: {normalizedSample}.");
                    TestContext.WriteLine($"Number of clicks: {numberOfClicks}");
                    TestContext.WriteLine("\n");

                    Thread.Sleep(normalizedSample);
                }
            }
        }

        [Test]
        public void ClickBellyButtonRepeateably() =>
            ClickBellyButton(1000);

        private static Random GenerateRandomWithSeed() =>
            new Random((int)(DateTime.Now.ToFileTime() % int.MaxValue));

        private static int GetIntNormalizedSample(Normal normal) =>
            (int)Math.Ceiling(GetDoubleNormalizedSample(normal));

        private static double GetDoubleNormalizedSample(Normal normal) =>
            Math.Abs(normal.Sample());
    }
}