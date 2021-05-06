using MarketUITest.ServerConnect;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MarketUITest
{
    [TestFixture]
    public class UITests
    {
        private const string WindowsAppDriverUrl = "http://127.0.0.1:4723";
        private const string AppPath = @"D:\С#\MyWork\bin\Debug\MarketWorkBD.exe";

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
            server.delete(name, price); //We ask the server to delete our object (you can delete it using the UI, but it's more cool)
            Assert.AreEqual("True", server.ReceiveMessage());
        }

        [Test]
        public void TestChange()
        {
            string name = "TestChange";
            string price = "213";

            //We check if we have anything to change (suddenly the last test broke everything)
            AppiumWebElement listView = driver.FindElementByAccessibilityId("listBoxSelect");
            IList<AppiumWebElement> items = listView.FindElementsByXPath(".//child::*");
            List<string> texts = new List<string>();
            foreach(AppiumWebElement item in items)
            {
                texts.Add(item.Text);
            }
            Assert.IsTrue(texts.Contains("1 мюсли 89"), "Не содержит элемента для теста");

            /// We change item
            driver.FindElementByName("UPDATE").Click();
            driver.FindElementByAccessibilityId("textBoxIdUpdate").SendKeys("1");
            driver.FindElementByAccessibilityId("updateTextboxName").SendKeys(name);
            driver.FindElementByAccessibilityId("updateTextboxPrice").SendKeys(price);
            driver.FindElementByAccessibilityId("updateButton").Click();

            //Go back and check that our changes have been accepted
            driver.FindElementByName("SELECT").Click();
            listView = driver.FindElementByAccessibilityId("listBoxSelect");
            listView.Click();
            items = listView.FindElementsByXPath(".//child::*");
            texts = new List<string>();
            foreach (AppiumWebElement item in items)
            {
                texts.Add(item.Text);
            }
            Assert.IsTrue(texts.Contains("1 " + name + " " + price), "Элемент не изменился");

            //Change the element back(what was done in the last test using the server side)
            driver.FindElementByName("UPDATE").Click();
            driver.FindElementByAccessibilityId("textBoxIdUpdate").SendKeys("1");
            driver.FindElementByAccessibilityId("updateTextboxName").SendKeys("мюсли");
            driver.FindElementByAccessibilityId("updateTextboxPrice").SendKeys("89");
            driver.FindElementByAccessibilityId("updateButton").Click();
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
                    server = null;
                }
                driver = null;
            }
        }
    }
}