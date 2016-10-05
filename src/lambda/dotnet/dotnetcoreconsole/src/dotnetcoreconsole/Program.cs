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
    }

    public class Program
    {
        private static readonly string WeblogFeedUri = "http://feeds.feedburner.com/AmazonWebServicesBlog";
        private static readonly string tabs = "     ";

        public static void Main(string[] args)
        {
            string xmlFeed = String.Empty;

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
            string stdin = "{\"stdin\": \"Empty\"}";

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
                    String resultString = await response.Content.ReadAsStringAsync();

                    Console.Out.WriteLine($"xmlStringResult: {resultString}");

                    XElement root = XElement.Load(result);

                    IEnumerable<XElement> entries =
                        from item in root.Descendants("item")
                        where item.Element("title").Value != String.Empty
                        select item;

                    foreach (XElement el in entries)
                    {
                        builder.AppendFormat(
                            "{0}{0}{{\n{0}{0}{0}\"title\": \"{1}\",\n{0}{0}{0}\"link\": \"{2}\",\n{0}{0}{0}\"description\": \"{3}\"\n {0}{0}}}{4}\n",
                            tabs,
                            (string) el.Element("title").Value,
                            (string) el.Element("link").Value,
                            (string) el.Element("description").Value,
                            el.ElementsAfterSelf().Any() ? "," : String.Empty);
                    }

                }
                catch (HttpRequestException e)
                {
                    Console.Error.WriteLine($"Exception: {e.Message}");
                }
            }

            builder.AppendLine("]");

            string builderOutput = builder.ToString();
            Console.Out.WriteLine($"Generated JSON: {builderOutput}");
            Console.Out.WriteLine();

            try
            {
                var output = JsonConvert.DeserializeObject<IEnumerable<Entry>>(builderOutput);
                //SetTerminalColors();
                //SetTerminalColors(backgroundColor: ConsoleColor.Red, foregroundColor: ConsoleColor.White);
                Console.Out.WriteLine("First three enteries of serialized JSON: ");
                Console.Out.WriteLine($"{JsonConvert.SerializeObject(output.Take(3))}");
                Console.Out.WriteLine();

            }
            catch (Exception e)
            {
                //SetTerminalColors(backgroundColor: ConsoleColor.Red, foregroundColor: ConsoleColor.White);
                Console.Error.WriteLine($"Exception: {e.Message}");
            }

            try
            {
                var output = JsonConvert.DeserializeObject<IEnumerable<Entry>>(stdin);
                //SetTerminalColors(backgroundColor: ConsoleColor.Red, foregroundColor: ConsoleColor.White);
                Console.Out.WriteLine("Serialized JSON from stdin (or default value): ");
                Console.Out.WriteLine($"{JsonConvert.SerializeObject(output)}");
                Console.Out.WriteLine();

            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Exception: {e.Message}");
            }
        }

        private static void SetTerminalColors(ConsoleColor backgroundColor = ConsoleColor.Blue, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
        }
    }
}