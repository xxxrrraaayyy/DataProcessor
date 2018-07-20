namespace DataProcessor
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Text;
    using HtmlAgilityPack;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    [TestClass]
    public class DataProcessor
    {
        [TestMethod]
        public async Task MyTestProcessorAsync()
        {
            //await DataRetrieverAsync();
            await GetPages();
            await LoadPages();
        }

        private int _pages;

        private HtmlDocument _webContent;

        private const string DIR = @"C:\xuexi\data.csv";

        /// <summary>
        ///     Async method to load the first page and return the number of total pages
        /// </summary>
        /// <returns></returns>
        public async Task GetPages()
        {
            var html = @"http://kaijiang.zhcw.com/zhcw/html/ssq/list_1.html";

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = await web.LoadFromWebAsync(html);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//body/table[@class='wqhgt']/tr");

            _pages = int.Parse(nodes[nodes.Count - 1].SelectSingleNode(".//strong[1]").InnerHtml);
        }

        public async Task LoadPages()
        {
            for(int p = _pages; p>0; p--)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("http://kaijiang.zhcw.com/zhcw/html/ssq/list_").Append(p).Append(".html");
                string url = sb.ToString();
                await LoadAndProcessContents(url);
            }
        }

        public async Task LoadAndProcessContents(string url)
        {
            HtmlWeb web = new HtmlWeb();
            _webContent = await web.LoadFromWebAsync(url);
            var nodes = _webContent.DocumentNode.SelectNodes("//body/table[@class='wqhgt']/tr");
            ProcessContents(nodes);
        }

        public void ProcessContents(HtmlNodeCollection nodes)
        {
            List<string> page = new List<string>();
            for (int i = nodes.Count-2; i > 1; i--)
            {
                StringBuilder row = new StringBuilder();
                var node = nodes[i].SelectNodes(".//td");
                row.Append(node[0].InnerHtml + ",");
                row.Append(node[1].InnerHtml + ",");
                var balls = node[2].SelectNodes(".//em");
                foreach(var number in balls)
                {
                    row.Append(number.InnerHtml + ",");
                }
                row.Append(node[3].SelectSingleNode(".//strong").InnerHtml.Replace(",", "") + ",");
                row.Append(node[4].SelectSingleNode(".//strong").InnerHtml + ",");
                row.Append(node[5].SelectSingleNode(".//strong").InnerHtml);

                page.Add(row.ToString());
            }
            WriteOutput(page.ToArray());
        }

        public void WriteOutput (string[] output)
        {
            try
            {
                File.AppendAllLines(DIR, output);
                File.AppendAllLines(DIR, new string[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            
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

            var nodes = htmlDoc.DocumentNode.SelectNodes("//body/table[@class='wqhgt']/tr");

            Console.WriteLine(nodes[nodes.Count-2].SelectNodes(".//td")[2].SelectNodes(".//em")[0].InnerHtml);

            StringBuilder row = new StringBuilder();
            var node = nodes[nodes.Count - 2].SelectNodes(".//td");
            row.Append(node[0].InnerHtml + ",");
            row.Append(node[1].InnerHtml + ",");
            var balls = node[2].SelectNodes(".//em");
            foreach (var number in balls)
            {
                row.Append(number.InnerHtml + ",");
            }
            row.Append(node[3].SelectSingleNode(".//strong").InnerHtml + ",");
            row.Append(node[4].SelectSingleNode(".//strong").InnerHtml + ",");
            row.Append(node[5].SelectSingleNode(".//strong").InnerHtml);

            Console.WriteLine(row.ToString());

            //StringBuilder csv = new StringBuilder();
            //csv.AppendLine(nodes[nodes.Count - 1].SelectSingleNode(".//strong[7]").InnerHtml);
            //csv.AppendLine(nodes[nodes.Count - 1].SelectSingleNode(".//strong[1]").InnerHtml);
            //File.WriteAllText(dir, csv.ToString());
            // To append more lines to the csv file
            //File.AppendAllText(dir, csv.ToString());
        }
    }
}
