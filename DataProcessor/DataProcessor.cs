namespace DataProcessor
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Net;
    using System.Text;
    using HtmlAgilityPack;

    [TestClass]
    public class DataProcessor
    {
        [TestMethod]
        public void MyTestProcessor()
        {
            //LoginCnblogs();
            //HtmlAgilityPackExample();
            DataRetriever();
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

        /// <summary>
        ///     Method to get data
        /// </summary>
        public void DataRetriever()
        {
            var html = @"http://kaijiang.zhcw.com/zhcw/html/ssq/list_1.html";

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//body/table[@class='wqhgt']");

            Console.WriteLine("Node Name: " + node.Name + "\n" + node.OuterHtml);
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
    }
}
