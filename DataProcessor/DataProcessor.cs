namespace DataProcessor
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Net;
    using System.Text;
    using HtmlAgilityPack;
    using System.Threading.Tasks;

    [TestClass]
    public class DataProcessor
    {
        [TestMethod]
        public async Task MyTestProcessorAsync()
        {
            await DataRetrieverAsync();
        }

        /// <summary>
        ///     Method to get data
        /// </summary>
        public async Task DataRetrieverAsync()
        {
            var html = @"http://kaijiang.zhcw.com/zhcw/html/ssq/list_1.html";


            HtmlWeb web = new HtmlWeb();

            var htmlDoc = await web.LoadFromWebAsync(html);
            //var htmlDoc = web.Load(html);

            //var page = htmlDoc.DocumentNode.SelectSingleNode("//body/table[@class='wqhgt']/tr");

            //Console.WriteLine("Node Name: " + page.Name + "\n" + page.OuterHtml);


            //var nodes = htmlDoc.DocumentNode.SelectNodes("//body/table[@class='wqhgt']/tr");

            //for(int i=2; i<nodes.Count; i++)
            //{
            //    Console.WriteLine("This is you dame node " + i);
            //    Console.WriteLine(nodes[i].OuterHtml);
            //}

            var nodes = htmlDoc.DocumentNode.SelectNodes("//body/table[@class='wqhgt']/tr");

            Console.WriteLine(nodes[4].SelectNodes(".//td")[0].InnerHtml);
            Console.WriteLine(nodes[4].SelectNodes(".//td")[1].InnerHtml);
            Console.WriteLine(nodes[4].SelectNodes(".//td")[2].InnerHtml);
            Console.WriteLine(nodes[4].SelectNodes(".//td")[3].InnerHtml);
            Console.WriteLine(nodes[4].SelectNodes(".//td")[4].InnerHtml);
            Console.WriteLine(nodes[4].SelectNodes(".//td")[5].InnerHtml);
            var numbers = nodes[4].SelectNodes(".//em");
            foreach(var n in numbers)
            {
                Console.WriteLine(n.InnerHtml);
            }

        }

        public async void LoadWeb(string url)
        {
            var html = @"http://kaijiang.zhcw.com/zhcw/html/ssq/list_1.html";

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = await web.LoadFromWebAsync(html);
        }


        private static String dir = @"C:\work\";

        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="html"></param>
        public static void Write(string fileName, string html)
        {
            try
            {
                FileStream fs = new FileStream(dir + fileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(html);
                sw.Close();
                fs.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }

        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="html"></param>
        public static void Write(string fileName, byte[] html)
        {
            try
            {
                File.WriteAllBytes(dir + fileName, html);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }

        public void HtmlAgilityPackExample()
        {
            string mainUrl = "http://www.shuichan.cc/";
            string xpath = "/html/body/center/table[4]/tr/td[1]/table[3]/tr[2]/td/table";

            WebClient wc = new WebClient();
            wc.BaseAddress = mainUrl;
            wc.Encoding = Encoding.GetEncoding("gb2312");
            HtmlDocument doc = new HtmlDocument();
            string html = wc.DownloadString("news_list.asp?action=more&c_id=162&s_id=271");
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("/html/body/center/table[4]/tr/td[1]/table[3]/tr[2]/td/table");

            HtmlNodeCollection CNodes = node.ChildNodes;

            foreach (var item in CNodes)
            {
                var obj = item.SelectSingleNode("td/a");
                Console.WriteLine("????:" + obj.InnerText);
                Console.WriteLine("????:" + mainUrl + obj.Attributes["href"].Value);
                Console.WriteLine("");
            }

            Console.ReadKey();
        }
    }
}
