using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AwsLambdaDotNet
{
    public class Entry
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
    }

    public class Program
    {
#if DEBUG
        private static readonly ConsoleColor CurrentBackgroundColor = Console.BackgroundColor;
        private static readonly ConsoleColor CurrentForegroundColor = Console.ForegroundColor;
        private static readonly ConsoleColor EmphasisBackgroundColor = ConsoleColor.White;
        private static readonly ConsoleColor EmphasisForegroundColor = ConsoleColor.DarkRed;
#endif

        private static readonly string WeblogFeedUri = "http://feeds.feedburner.com/AmazonWebServicesBlog";
        private static readonly string tabs = "     ";
        private static Entry[] entries = new Entry[]
        {
            new Entry { Description = "This is description one", 
                        Link = "http://example.com/entry/1", 
                        Title = "Entry One",
                        Date = "Tue, 4 Oct 2016 16:21:28 +0000" },
            new Entry { Description = "This is description two", 
                        Link = "http://example.com/entry/2", 
                        Title = "Entry Two",
                        Date = "Wed, 5 Oct 2016 12:52:37 +0000"  },
            new Entry { Description = "This is description three", 
                        Link = "http://example.com/entry/3", 
                        Title = "Entry Three",
                        Date = "Fri, 7 Oct 2016 17:24:17 +0000" }
        };

        public static void Main(string[] args)
        {
            string xmlFeed = string.Empty;

            try
            {
                xmlFeed = args[0];
            }
            catch
            {
                xmlFeed = WeblogFeedUri;
            }

            GetXmlFeedAsync(xmlFeed).Wait();
        }

        private static async Task GetXmlFeedAsync(string weblogFeedUri)
        {
            string stdin = "[{\"stdin\": \"Empty\"}]";

            if (Console.IsInputRedirected)
            {
                stdin = await Console.In.ReadToEndAsync();
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("[");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(weblogFeedUri);
                    response.EnsureSuccessStatusCode();

                    Stream result = await response.Content.ReadAsStreamAsync();
                    string resultString = await response.Content.ReadAsStringAsync();

                    WriteHeaderAndResultToConsole("xmlStringResult: ", resultString);

                    XElement root = XElement.Load(result);

                    IEnumerable<XElement> entries =
                        from item in root.Descendants("item")
                        where item.Element("title").Value != string.Empty
                        select item;

                    foreach (XElement el in entries)
                    {
                        builder.AppendFormat(
                            "{0}{0}{{\n{0}{0}{0}\"title\": \"{1}\",\n{0}{0}{0}\"link\": \"{2}\",\n{0}{0}{0}\"date\": \"{3}\",\n{0}{0}{0}\"description\": \"{4}\"\n {0}{0}}}{5}\n",
                            tabs,
                            (string) el.Element("title").Value,
                            (string) el.Element("link").Value,
                            (string) el.Element("pubDate").Value,
                            (string) el.Element("description").Value,
                            el.ElementsAfterSelf().Any() ? "," : string.Empty);
                    }

                }
                catch (HttpRequestException e)
                {
                    Console.Error.WriteLine($"Exception: {e.Message}");
                }
            }

            builder.AppendLine("]");

            string builderOutput = builder.ToString();
            WriteHeaderAndResultToConsole("Generated JSON: ", builderOutput);

            try
            {
                var output = JsonConvert.DeserializeObject<IEnumerable<Entry>>(builderOutput);
                WriteHeaderAndResultToConsole("First three enteries of serialized JSON: ", JsonConvert.SerializeObject(output.Take(3)));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Exception: {e.Message}");
            }

            try
            {
                var output = JsonConvert.DeserializeObject<IEnumerable<Entry>>(stdin);
                WriteHeaderAndResultToConsole("Serialized JSON from stdin (or default value): ", JsonConvert.SerializeObject(output));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Exception: {e.Message}");
            }
        }

        private static void WriteHeaderAndResultToConsole(string header, string result)
        {
            Console.Out.WriteLine();
#if DEBUG
            SetTerminalColors(backgroundColor: CurrentBackgroundColor, foregroundColor: EmphasisForegroundColor);
#endif
            Console.Out.WriteLine(header);
#if DEBUG
            ResetTerminalColors();
#endif
            Console.Out.WriteLine(result);
        }
        private static void SetTerminalColors(ConsoleColor backgroundColor = ConsoleColor.Blue, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            #if DEBUG
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            #endif
        }

        private static void ResetTerminalColors()
        {
            #if DEBUG
            Console.BackgroundColor = CurrentBackgroundColor;
            Console.ForegroundColor = CurrentForegroundColor;
            #endif
        }
    }
}