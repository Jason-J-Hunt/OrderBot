using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace OrderBot
{
    public class Watcher{
        //Fields
        #region fields
        public string watcherName {get; set;}
        public List<String> phoneNumbers {get; set;}
        public List<String> urls {get; set;}
        //List of browsers for each given url from config
        public List<IWebDriver> webDrivers {get; set;}
//url is key xpath is value
        public IDictionary<string, string> urlXPath {get; set;}
        #endregion
        //constructor takes watcher name and path to config
        public Watcher(string watcherName){
            //set watcher name
            this.watcherName = watcherName;
            //instantiate lists
            this.phoneNumbers = new List<String>();
            this.urls = new List<String>();
            this.webDrivers = new List<IWebDriver>();
            //instantiate map
            urlXPath =  new Dictionary<string, string>();
        }
        private void createWebDrivers(){
            foreach(var url in urls){
                var driver = new ChromeDriver();
                driver.Url = url;
                //add driver to list
                webDrivers.Add(driver);
            }
        }
        public void refresh(){
            //go through driver and refresh 
            string xpath = "";

            foreach(var driver in webDrivers){
                driver.Navigate().Refresh();
                if(urlXPath.TryGetValue(driver.Url, out xpath)){
                    var element = driver.FindElement(By.XPath(xpath));
                    Console.WriteLine(element.Text);
                }
                else{
                    Console.WriteLine($"XPath for Url:{driver.Url} not in map check config file!");
                }
            }
        }
    }

}