using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace csdn_article_download
{
    internal class Program
    {
        public class Article_Result
        {
            public string body { get; set; }
            public string title { get; set; }
        }
        public static string[] USER_AGENT_LIST =
        {
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/22.0.1207.1 Safari/537.1",
        "Mozilla/5.0 (X11; CrOS i686 2268.111.0) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.57 Safari/536.11",
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6",
        "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1090.0 Safari/536.6",
        "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; 360SE)",
        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.1 Safari/536.3",
        "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.1 Safari/536.3",
        "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.0 Safari/536.3",
        "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.24 (KHTML, like Gecko) Chrome/19.0.1055.1 Safari/535.24",
        "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/535.24 (KHTML, like Gecko) Chrome/19.0.1055.1 Safari/535.24",
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.82 Safari/537.36"
        };
        public static WebClient client = new WebClient();
        public static string CleanFileName(string fileName)
        {
            string pattern = @"[\\/:*?""<>|]";
            return Regex.Replace(fileName, pattern, "");
        }
        public static string Get_String(string url)
        {
            Random random = new Random();
            string header = USER_AGENT_LIST[random.Next(USER_AGENT_LIST.Length)];
            client.Headers.Add("user-agent", header);
            return client.DownloadString(url);
        }
        public static Article_Result Parse_Html(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var div = doc.GetElementbyId("content_views");
            string title = doc.DocumentNode.SelectSingleNode("//title").InnerText;
            return new Article_Result { body = div?.OuterHtml, title = title };

        }
        public static void Write_Html(Article_Result ar)
        {
            if (!Directory.Exists("./htmldownload"))
            {
                Directory.CreateDirectory("./htmldownload");
            }
            File.WriteAllText("./htmldownload/" + CleanFileName(ar.title) + ".html" , ar.body);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("输入csdn文章网址");
            string ans = Console.ReadLine();
            string Csdn_Html = Get_String(ans);
            Write_Html(Parse_Html(Csdn_Html));

        }
    }
}
