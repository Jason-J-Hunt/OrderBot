using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using CommandLine;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace OrderBot
{
    //commandline parsing
    class Options
    {
        [Option('c', "config", Required = true, HelpText = "Path to config file")]
        public string config { get; set; }

    }
    class Program
    {
        private static List<Watcher> watchers {get; set;}

        static void Main(string[] args)
        {
            Options ops = null;

             Console.CancelKeyPress += delegate {
                // call methods to clean up
                cleanup();
            };

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(parsed => ops = parsed);
            if(ops != null){
                watcher_initalize(ops);
                run();
            }
            
            Console.WriteLine("Hello World!");
        }

        private static void watcher_initalize(Options ops){
            watchers = new List<Watcher>();
            //open config
            using(StreamReader reader = new StreamReader(ops.config)){
                string line;
                string name = "";
                Watcher watcher = null;
                //read line by line
                while((line = reader.ReadLine()) != null){
                    //skip over comments
                    if(line.StartsWith("###"))
                        continue;
                    //create new watcher with this name
                    if(line.StartsWith("name:")){
                        //create new watcher
                        name = line.Substring(5);
                        Console.WriteLine($"Creating new {name}: Watcher...");
                        watcher = new Watcher(name);
                        //add watcher to list
                        watchers.Add(watcher);
                    }
                    else if(line.StartsWith("url:")){
                        var pair = line.Substring(4).Split('|');
                        //add url
                        Console.WriteLine($"adding url {pair[0]} to watcher {watcher.Name}");
                        watcher.urls.Add(pair[0].Trim());

                        //add url xpath pair
                        Console.WriteLine($"adding xpath {pair[1]} to watcher {watcher.Name}");
                        watcher.urlXPath.Add(pair[0].Trim(), pair[1].Trim());

                    }
                    else if(line.StartsWith("numbers:")){
                        var numbers = line.Substring(8).Split(':');
                        foreach(var number in numbers){
                            Console.WriteLine($"Adding number {number} to Watcher {watcher.Name}");
                            watcher.phoneNumbers.Add(number.Trim());
                        }
                    }

                }
            }
            //create Browser Instances for each watcher
            foreach(var watcher in watchers){
                watcher.createWebDrivers();
            }
        }

        private static void cleanup(){
            int count = 1;

            foreach(var watcher in watchers){
                //close each driver
                foreach(var driver in watcher.webDrivers){
                    driver.Quit();
                }
            }
            //kill chrome driver processes
            Process[] chromeDriverProcesses =  Process.GetProcessesByName("chromedriver");

            foreach(var chromeDriverProcess in chromeDriverProcesses)
            {
                Console.WriteLine($"Killing chromeDriver {count}...");
                chromeDriverProcess.Kill();
                count++;
            }
        }

        private static void run(){
            while(true){
                Console.WriteLine("Refreshing WebPages...");

                foreach(var watcher in watchers){
                    watcher.Refresh();
                }

                //Sleep for 5 min before checking again
                Console.WriteLine("Sleeping for 5 min...");
                System.Threading.Thread.Sleep(TimeSpan.FromMinutes(5));
            }
        }
    }
}
