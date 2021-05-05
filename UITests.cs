using MarketUITest.ServerConnect;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace MarketUITest
{
    public class UITests
    {
        private const string WindowsAppDriverUrl = "http://127.0.0.1:4723";
        private const string AppPath = @"D:\Ñ#\MyWork\bin\Debug\MarketWorkBD.exe";

        private static WindowsDriver<WindowsElement> driver;
        private static APIServer server;

        [SetUp]
        public void Setup()
        {
            if (driver == null)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", AppPath);
                appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                driver = new WindowsDriver<WindowsElement>(new Uri(WindowsAppDriverUrl), appiumOptions);

                if(server == null)
                {
                    server = new APIServer();
                }
            }
        }

        [Test]
        public void TestInsert()
        {
            string name = "TestInsert";
            string price = "1232";

            driver.FindElementByName("INSERT").Click();
            driver.FindElementByAccessibilityId("insertTextBoxName").SendKeys(name);
            driver.FindElementByAccessibilityId("insertTextBoxPrice").SendKeys(price);
            driver.FindElementByAccessibilityId("changeButton").Click();
            Thread.Sleep(1000);
            server.delete(name, price);
            Assert.AreEqual("True", server.ReceiveMessage());
        }

        [TearDown]
        public static void CleanUp()
        {
            if (driver != null)
            {
                driver.Close();
                driver.Quit();
                if(server != null)
                {
                    server.Disconnect();
                }
            }
        }
    }
}