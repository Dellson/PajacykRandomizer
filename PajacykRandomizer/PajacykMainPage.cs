using System;
using System.IO;
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
                    double stddev = 17000;
                    int minimumDelay = 8000;
                    int normalizedSample = MathHelper.GetIntNormalizedSample(new Normal(mean, stddev, randomizer));
                    normalizedSample = normalizedSample > minimumDelay ? normalizedSample : minimumDelay;
                    var path = @"C:\Projects\PajacykRandomizer-master\test-output.txt";

                    try
                    {
                        action.MoveToElement(bellyButton).Perform();
                        action.Click(bellyButton).Perform();
                    }
                    catch (NoSuchElementException) { }

                    File.AppendAllLines(path, new[] { $"{DateTime.Now}: Clicker number of clicks: {i}" });
                    Thread.Sleep(normalizedSample);
                }
            }
        }

        [Test]
        public void ClickBellyButtonRepeateably() =>
            ClickBellyButton(1000);
    }
}
