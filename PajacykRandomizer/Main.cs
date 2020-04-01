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
    public class Main
    {
        private static IWebDriver firefoxDriver;
        private static PajacykPage pajacykPage;
        private static Random randomizer;

        [SetUp]
        public static void SetUp()
        {
            //firefoxDriver = new FirefoxDriver();
            pajacykPage = new PajacykPage(firefoxDriver);
            randomizer = GenerateRandomWithSeed();
        }

        [TearDown]
        public static void TearDown() =>
            GenerateRandomWithSeed();

        [Test]
        public void ClickBellyButton()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                Actions action = new Actions(driver);
                FirefoxOptions options = new FirefoxOptions();
                options.AddArguments("--headless");
                int normalizedSample = -1;

                randomizer = GenerateRandomWithSeed();

                Normal normalDistBeforeClick = new Normal(200, 165, randomizer);
                Normal normalDistAfterClick = new Normal(5000, 2000, randomizer);

                PajacykPage pajacykPage = new PajacykPage(driver);
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(PajacykPage.mainUrl);

                var deactivatedBellyButton = pajacykPage
                    .GetDeactivatedBellyButton();

                action.MoveToElement(deactivatedBellyButton).Perform();

                // need to optimize randomization method using modified normal distribution
                // also, add dynamic writing output to a file
                normalizedSample = GetIntNormalizedSample(normalDistBeforeClick);

                TestContext.WriteLine($"Normalized sample after click: {normalizedSample}.");
                Thread.Sleep(normalizedSample);

                pajacykPage
                    .GetActivatedBellyButton()
                    .Click();

                normalizedSample = GetIntNormalizedSample(normalDistAfterClick);

                TestContext.WriteLine($"Normalized sample before click: {normalizedSample}.");
                Thread.Sleep(normalizedSample);
            }
        }

        [Test]
        public void RepeateableClickBellyButton()
        {
            for (int i = 0; i < 2; i++)
            {
                TestContext.WriteLine($"ClickBellyButton run start. Number of runs so far: {i}.");
                ClickBellyButton();
            }
        }

        private void LoadPage() => 
            firefoxDriver.Navigate().GoToUrl(PajacykPage.mainUrl);

        private static Random GenerateRandomWithSeed() =>
            new Random((int)(DateTime.Now.ToFileTime() % int.MaxValue));

        private static int GetIntNormalizedSample(Normal normal) =>
            (int)Math.Ceiling(GetDoubleNormalizedSample(normal));

        private static double GetDoubleNormalizedSample(Normal normal)
        {
            var sample = normal.Sample();
            return Math.Abs(sample);
        }

        private static void CloseBrowser() =>
            firefoxDriver.Quit();
    }
}