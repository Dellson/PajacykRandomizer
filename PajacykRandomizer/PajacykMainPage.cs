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
                    int delayBeforeReadingClickCount = 8000;
                    int normalizedSample = MathHelper.GetIntNormalizedSample(new Normal(mean, stddev, randomizer));
                    normalizedSample = normalizedSample > delayBeforeReadingClickCount ? normalizedSample : delayBeforeReadingClickCount;
                    var path = @"C:\Users\Paweł Dela\source\repos\PajacykRandomizer\test-output.txt";

                    action.MoveToElement(bellyButton).Perform();
                    action.Click(bellyButton).Perform();

                    Thread.Sleep(delayBeforeReadingClickCount);
                    string numberOfClicks = pajacykPage.GetClicksNumber();

                    var line1 = "";//$"Normalized sample after click: {normalizedSample}.";
                    var line2 = $"Total number of clicks: {numberOfClicks}";
                    var line3 = $"Clicker number of clicks: {i}";
                    TestContext.WriteLine(line1);
                    TestContext.WriteLine(line2);
                    TestContext.WriteLine(line3);
                    File.AppendAllLines(path, new string[3] { line1, line2, line3 });

                    Thread.Sleep(normalizedSample);
                }
            }
        }

        [Test]
        public void ClickBellyButtonRepeateably() =>
            ClickBellyButton(3);
    }
}
