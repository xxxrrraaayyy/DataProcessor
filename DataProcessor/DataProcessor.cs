namespace DataProcessor
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
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

            HtmlNodeCollection CNodes = node.ChildNodes;    //??????

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

        /// <summary>
        /// ?????
        /// </summary>
        public static void LoginCnblogs()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            String url = "http://passport.cnblogs.com/login.aspx";
            HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            String result = response.Content.ReadAsStringAsync().Result;

            String username = "hi_amos";
            String password = "??";

            do
            {
                String __EVENTVALIDATION = new Regex("id=\"__EVENTVALIDATION\" value=\"(.*?)\"").Match(result).Groups[1].Value;
                String __VIEWSTATE = new Regex("id=\"__VIEWSTATE\" value=\"(.*?)\"").Match(result).Groups[1].Value;
                String LBD_VCID_c_login_logincaptcha = new Regex("id=\"LBD_VCID_c_login_logincaptcha\" value=\"(.*?)\"").Match(result).Groups[1].Value;

                //?????
                url = "http://passport.cnblogs.com" + new Regex("id=\"c_login_logincaptcha_CaptchaImage\" src=\"(.*?)\"").Match(result).Groups[1].Value;
                response = httpClient.GetAsync(new Uri(url)).Result;
                Write("amosli.png", response.Content.ReadAsByteArrayAsync().Result);

                Console.WriteLine("???????:");
                String imgCode = "wupve";//????????,??????
                imgCode = Console.ReadLine();

                //????
                url = "http://passport.cnblogs.com/login.aspx";
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                paramList.Add(new KeyValuePair<string, string>("__EVENTTARGET", ""));
                paramList.Add(new KeyValuePair<string, string>("__EVENTARGUMENT", ""));
                paramList.Add(new KeyValuePair<string, string>("__VIEWSTATE", __VIEWSTATE));
                paramList.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", __EVENTVALIDATION));
                paramList.Add(new KeyValuePair<string, string>("tbUserName", username));
                paramList.Add(new KeyValuePair<string, string>("tbPassword", password));
                paramList.Add(new KeyValuePair<string, string>("LBD_VCID_c_login_logincaptcha", LBD_VCID_c_login_logincaptcha));
                paramList.Add(new KeyValuePair<string, string>("LBD_BackWorkaround_c_login_logincaptcha", "1"));
                paramList.Add(new KeyValuePair<string, string>("CaptchaCodeTextBox", imgCode));
                paramList.Add(new KeyValuePair<string, string>("btnLogin", "?  ?"));
                paramList.Add(new KeyValuePair<string, string>("txtReturnUrl", "http://home.cnblogs.com/"));
                response = httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList)).Result;
                result = response.Content.ReadAsStringAsync().Result;
                Write("myCnblogs.html", result);
            } while (result.Contains("?????,???????"));

            Console.WriteLine("????!");

            //???????
            httpClient.Dispose();
        }
    }
}
