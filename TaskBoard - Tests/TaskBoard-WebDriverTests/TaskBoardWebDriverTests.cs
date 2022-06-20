using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace TaskBoard.WebDriverTests
{
    public class WebDriverTests
    {
        private const string url = "https://taskboard.nakov.repl.co";
        private WebDriver driver;

        [SetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_ListAllTasks_CheckFirstTask()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            var taskLink = driver.FindElement(By.LinkText("Task Board"));

            // Act
            taskLink.Click();

            // Assert
            var title = driver.FindElement(By.XPath("//div[3]/table[1]/tbody/tr[1]/th")).Text;
            var description = driver.FindElement(By.XPath("//div[3]/table[1]/tbody/tr[1]/td")).Text;

            Assert.That(title, Is.EqualTo("Title"));
            Assert.That(description, Is.EqualTo("Project skeleton"));
        }

        [Test]
        public void Test_SearchTasks_CheckFirstResults()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            // Act
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("home");
            driver.FindElement(By.Id("search")).Click();


            // Assert
            var title = driver.FindElement(By.CssSelector("tr.title > th")).Text;
            var description = driver.FindElement(By.CssSelector("tr.title > td")).Text;

            Assert.That(title, Is.EqualTo("Title"));
            Assert.That(description, Is.EqualTo("Home page"));
        }

        [Test]
        public void Test_SearchTasks_EmptyResult()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();

            // Act
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("missing{randnum}");
            driver.FindElement(By.Id("search")).Click();


            // Assert
            var resultLabel = driver.FindElement(By.Id("searchResult")).Text;
            Assert.That(resultLabel, Is.EqualTo("No tasks found."));
        }

        [Test]
        public void Test_CreateTasks_CreateWithInvalidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();


            // Act
            var description = driver.FindElement(By.Id("description"));
            description.SendKeys("Alabala");
            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();


            // Assert
            var errorMessage = driver.FindElement(By.CssSelector("div.err")).Text;
            Assert.That(errorMessage, Is.EqualTo("Error: Title cannot be empty!"));
        }

        [Test]
        public void Test_CreateTask_CreateWithValidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var titleSend = "Alabala" + DateTime.Now.Ticks;
            var descriptionSend = "Alabala" + DateTime.Now.Ticks;

            // Act
            driver.FindElement(By.Id("title")).SendKeys(titleSend);
            driver.FindElement(By.Id("description")).SendKeys(descriptionSend);

            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();


            // Assert
            var taskTitle = driver.FindElements(By.XPath("/html/body/main/div/div[1]/table/tbody/tr[1]/td")).Last().Text;
            var taskDescription = driver.FindElements(By.XPath("/html/body/main/div/div[1]/table/tbody/tr[2]/td/div")).Last().Text;

            Assert.That(taskTitle, Is.EqualTo(titleSend));
            Assert.That(taskDescription, Is.EqualTo(descriptionSend));
           
        }
    }
}