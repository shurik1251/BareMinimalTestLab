using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using TechTalk.SpecFlow;

namespace TheApp.UiTests
{
    [Binding]
    public class HomePageSteps
	{
		private IWebDriver _driver;

		[Given(@"I have entered the home page")]
        public void GivenIHaveEnteredTheHomePage()
        {
			_driver = new ChromeDriver();
			_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
			_driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
			_driver.Manage().Window.Maximize();
			_driver.Navigate().GoToUrl("http://localhost:25688");
		}
        
        [Then(@"the result should be ""(.*)"" on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(string pupilName)
        {
			var actualUser = _driver.FindElement(By.CssSelector("p.pupil-row")).Text;
			actualUser.Should().Equals(pupilName);
			_driver.Close();
			_driver.Quit();
		}
    }
}
