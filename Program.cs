﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using CommandLine;
using System.Collections.Generic;
using System.IO;

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
            Options ops = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(parsed => ops = parsed);
            watcher_initalize(ops.config);
            Console.WriteLine("Hello World!");
        }

        private static void watcher_initalize(string config_path){
            watchers = new List<Watcher>();
            //open config
            using(StreamReader reader = new StreamReader(config_path)){
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
                        var pair = line.Substring(4).Split(':');
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
    }
}
