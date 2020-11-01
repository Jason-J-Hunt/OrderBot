using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

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
            //create single driver uses less mem
            var driver = new ChromeDriver(@"D:\CodeProjects\OrderBot-1\drivers");
            
            webDrivers.Add(driver);

        }
        public void Refresh(){

            foreach(var driver in webDrivers){
                //check each webpage to see if item is instock
                foreach(var url in urls){
                    Console.WriteLine($"Page Refreshed for {driver.Url} ...");
                    driver.Navigate().GoToUrl(url);
                    Alert(driver);
                }

            }
        }
        //check if instock and alert numbers with url
        private void Alert(IWebDriver driver){
            string xpath = "";
            bool alert = false;
            IWebElement element = null;

            Console.WriteLine("Waiting 5 seconds for page to load...");
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            Console.WriteLine("Waiting Done...");

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
                foreach(var number in phoneNumbers){
                    var message = MessageResource.Create(
                        body: $"Message from Jason's Bot: {Name}\n Item in stock: {driver.Url}",
                        from: new Twilio.Types.PhoneNumber("+12054090597"),
                        to: new Twilio.Types.PhoneNumber(number)
                    );

                    Console.WriteLine(message.Sid);
                }
                return;
            }
        }
    }

}