using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumDemo
{
    class Demo
    {
        IWebDriver driver;

        [Test]
        public void SimpleTest()
        {
            driver = new ChromeDriver("Actual directory to chromedrive.eve");
            driver.Url = "https://www.linkedin.com/in/calvindeng/";
            driver.Manage().Window.Maximize();

            string GithubProfile = "/html/body/main/section[1]/section/section[1]/div/div[1]/div[2]/div/div[3]/a/span";
            IWebElement link = driver.FindElement(By.XPath(GithubProfile));
            link.Click();

            driver.Close();

        }
    }
}
