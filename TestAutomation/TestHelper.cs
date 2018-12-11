using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAutomation
{
	public static class TestHelper
	{
		public static IWebElement FindElement(this IWebDriver driver, By by, TimeSpan timeout)
		{
			if (timeout.Seconds > 0)
			{
				var wait = new WebDriverWait(driver, timeout);
				return wait.Until(Condition(by));
			}
			return driver.FindElement(by);
		}

		public static IWebDriver GetChromeDriver()
		{
			ChromeOptions options = new ChromeOptions();
			IWebDriver driver = new ChromeDriver(options);
			return driver;
		}

		public static Func<IWebDriver, IWebElement> Condition(By locator)
		{
			return (driver) => {
				var element = driver.FindElements(locator).FirstOrDefault();
				return element != null && element.Displayed && element.Enabled ? element : null;
			};
		}

		public static void Login(IWebDriver driver)
		{
			var userNameElement = FindElement(driver, By.CssSelector("[ng-model='user.name']"), TimeSpan.FromSeconds(10));
			userNameElement.SendKeys("admin");
			var userPasswordElement = FindElement(driver, By.CssSelector("[ng-model='user.password']"), TimeSpan.FromSeconds(10));
			userPasswordElement.SendKeys("123");

			var loginButton = FindElement(driver, By.Id("btnLogin"), TimeSpan.FromSeconds(10));
			loginButton.Click();
		}
		public static void Wait(IWebDriver driver, TimeSpan delay)
		{
			double interval = 250;
			var now = DateTime.Now;
			var wait = new WebDriverWait(driver, delay);
			wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
			wait.Until(wd => (DateTime.Now - now) - delay > TimeSpan.Zero);
		}

		public static void WaitLoading(IWebDriver driver)
		{
			Wait(driver, TimeSpan.FromSeconds(5));
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(500));
			wait.Until(d => !d.FindElement(By.CssSelector("[is-loading='loading']")).Displayed);
		}

		public static void AnalyzeClick(IWebDriver driver, string url)
		{
			driver.Navigate().GoToUrl(url);
			Login(driver);
			WaitLoading(driver);
			var element = FindElement(driver, By.CssSelector("[ng-show='AnalysisVisible']"), TimeSpan.FromSeconds(10));

			element.Click();
		}

	}
}
