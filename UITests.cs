using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace MarketUITest
{
    public class Tests
    {
        private const string WindowsAppDriverUrl = "http://127.0.0.1:4723";
        private const string AppPath = @"D:\Ñ#\MyWork\bin\Debug\MarketWorkBD.exe";

        private static WindowsDriver<WindowsElement> driver;

        [SetUp]
        public void Setup()
        {
            if (driver == null)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", AppPath);
                appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                driver = new WindowsDriver<WindowsElement>(new Uri(WindowsAppDriverUrl), appiumOptions);
            }
        }

        [Test]
        public void Test1()
        {
            driver.FindElementByName("INSERT").Click();
            driver.FindElementByAccessibilityId("insertTextBoxName").SendKeys("Test");
            Thread.Sleep(3000);
        }

        [TearDown]
        public static void CleanUp()
        {
            if (driver != null)
            {
                driver.Close();
                driver.Quit();
            }
        }
    }
}