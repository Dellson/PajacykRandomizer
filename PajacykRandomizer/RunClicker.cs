using System;
using System.Configuration;
using System.IO;
using System.Threading;
using MathNet.Numerics.Distributions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace PajacykRandomizer
{
    [TestFixture]
    public class RunClicker
    {
        private FirefoxOptions options;
        private string url;
        private string path;

        [SetUp]
        public void SetUp()
        {
            url = GetString(nameof(url));
            path = GetString(nameof(path));
            options = new FirefoxOptions();
            options.AddArguments("--headless");
        }

        [Test]
        [TestCase(10)]
        public void ClickBellyButton(int loopCount)
        {
            int errorCount = 0;

            using (IWebDriver driver = new FirefoxDriver(options))
            {
                driver.Navigate().GoToUrl(url);

                for (int i = 1; i <= loopCount; i++)
                {
                    try
                    {
                        IWebElement button = driver.FindElement(By.ClassName("pajacyk__clickbox"));
                        button.Click();
                    }
                    catch (NoSuchElementException)
                    {
                        errorCount++;
                    }

                    File.AppendAllText(path, $"{DateTime.Now}: Number of clicks: {(i - errorCount).ToString().PadLeft(4, '0')}\n");
                    Thread.Sleep(GetIntSample());
                }
            }
        }

        public static int GetIntSample()
        {
            var normal = new Normal(
                GetDouble("mean"),
                GetDouble("stddev"),
                new Random());

            return (int)Math.Abs(normal.Sample());
        }

        public static string GetString(string key) =>
            ConfigurationManager.AppSettings[key];

        public static double GetDouble(string key) =>
            Convert.ToDouble(ConfigurationManager.AppSettings[key]);
    }
}

