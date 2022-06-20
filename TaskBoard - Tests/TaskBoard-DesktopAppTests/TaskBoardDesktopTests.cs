using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;

namespace TaskBoard.DesktopClientTests
{
    public class DesktopTest
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string TaskBoardUrl = "https://taskboard.nakov.repl.co/api";
        private const string appLocation = @"C:\TaskBoard\TaskBoard.DesktopClient.exe";

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }


        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_SearchTasks_VerifyFirstResult()
        {
            // Arrange
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(TaskBoardUrl);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

          
            //Act
            var buttonSearch = driver.FindElementByAccessibilityId("buttonSearch");
            buttonSearch.Click();

            Thread.Sleep(3000);

            var addButton = driver.FindElementByAccessibilityId("buttonAdd");
            addButton.Click();

            var textBoxTitle = driver.FindElementByAccessibilityId("textBoxTitle");
            textBoxTitle.SendKeys("Alabala123456789");

            var descripton = driver.FindElementByAccessibilityId("textBoxDescription");
            descripton.SendKeys("Alabala123" + DateTime.Now.Ticks);

            var buttonCreate = driver.FindElementByAccessibilityId("buttonCreate");
            buttonCreate.Click();

            var sendKey = driver.FindElementByAccessibilityId("textBoxSearchText");
            sendKey.SendKeys("Alabala123456789");
            buttonSearch.Click();

            Thread.Sleep(3000);

            var buttonReload = driver.FindElementByAccessibilityId("buttonReload");
            buttonReload.Click();

            Thread.Sleep(5000);

            sendKey.SendKeys("Project skeleton");
            buttonSearch.Click();


            // Assert

            var title = driver.FindElement(By.XPath("//Group[@Name=\"Done\"][starts-with(@AutomationId,\"ListViewGroup-\")]")).Text;
            Assert.That(title, Is.EqualTo("Done"));

            var firstResult = driver.FindElement(By.XPath("//Group[@Name=\"Done\"][starts-with(@AutomationId,\"ListViewGroup-\")]/ListItem[@Name=\"1\"]"));
           Assert.That(firstResult.Text, Is.EqualTo("1"));


            var searchResult = driver.FindElement(By.XPath("//StatusBar[@AutomationId=\"statusStrip\"]/Text[@Name=\"status box\"]"));
            Assert.That(searchResult != null);
        }
    }
}