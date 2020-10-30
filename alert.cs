using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace OrderBot
{
    public class Watcher{
        //Fields
        #region fields
        public string Name {get; set;}
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
            this.Name = watcherName;
            //instantiate lists
            this.phoneNumbers = new List<String>();
            this.urls = new List<String>();
            this.webDrivers = new List<IWebDriver>();
            //instantiate map
            urlXPath =  new Dictionary<string, string>();
        }
        public void createWebDrivers(){
            foreach(var url in urls){
                var driver = new ChromeDriver(@"D:\CodeProjects\OrderBot-1\drivers");
                driver.Url = url;
                //add driver to list
                webDrivers.Add(driver);
                Console.WriteLine($"New Driver Created for{url}");
            }
        }
        public void Refresh(){

            foreach(var driver in webDrivers){
                Console.WriteLine($"Page Refreshed for {driver.Url} ...");
                driver.Navigate().Refresh();
                Alert(driver);

            }
        }
        //check if instock and alert numbers with url
        private void Alert(IWebDriver driver){
            string xpath = "";
            bool alert = false;
            IWebElement element = null;
            if(urlXPath.TryGetValue(driver.Url, out xpath)){
                try{
                    element = driver.FindElement(By.XPath(xpath));
                    Console.WriteLine(element.Text);
                }
                catch(Exception e){
                    Console.WriteLine(e.ToString());
                    return;
                }
            }
            else{
                Console.WriteLine($"XPath for Url:{driver.Url} not in map check config file!");
                return;
            }

            if(element == null){
                Console.WriteLine($"XPath {xpath} not found for url {driver.Url}");
                return;
            }

            switch(element.Text.ToLower()){
                case "add to cart":
                    alert = true;
                    break;
                case "pre-order":
                    alert = true;
                    break;
                case "buy now":
                    alert = true;
                    break;
                default:
                    break;
            }
            //send text message to number with url for each website thats in stock
            if(alert){

            }
        }
    }

}