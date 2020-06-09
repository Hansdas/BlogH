using Blog.Common;
using Blog.Domain;
using HtmlAgilityPack;
using Quartz;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Quartz
{
    /// <summary>
    /// 爬取新闻定时器
    /// </summary>
    public class NewsQuartz : IJob
    {
        private INewsRepository _newsRepository;
        public NewsQuartz(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        enum NewsOrigin
        {
            知乎,
            游侠咨询,
            搜狗微信,
            CSDN,
            新浪新闻
        }
        class NewsUrlItem
        {
            public NewsUrlItem(string httpUrl, NewsOrigin newsOrigin)
            {
                HttpUrl = httpUrl;
                NewsOrigin = newsOrigin;
            }
            public string HttpUrl { get; set; }
            public NewsOrigin NewsOrigin { get; set; }
        }
        /// <summary>
        /// 爬取新闻
        /// </summary>
        public async Task StartNewsJob()
        {
            IList<NewsUrlItem> httpUrls = new List<NewsUrlItem>() {
               new NewsUrlItem("https://daily.zhihu.com",NewsOrigin.知乎) ,
               new NewsUrlItem("https://www.ali213.net/news/game",NewsOrigin.游侠咨询),
               new NewsUrlItem("https://weixin.sogou.com",NewsOrigin.搜狗微信) ,
               new NewsUrlItem("https://www.csdn.net",NewsOrigin.CSDN)  ,
               new NewsUrlItem("https://news.sina.com.cn/world",NewsOrigin.新浪新闻)
            };
            IList<News> newsList = new List<News>();
            foreach (var item in httpUrls)
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(item.HttpUrl);
                response.EnsureSuccessStatusCode();
                string html = response.Content.ReadAsStringAsync().Result;
                HtmlDocument document = new HtmlDocument();
                HtmlNode node = null;
                News news = null;
                switch (item.NewsOrigin)
                {
                    case NewsOrigin.知乎:
                        document.LoadHtml(html);
                        node = document.DocumentNode.SelectNodes("//a[@class='link-button']")[0];
                        news = new News(
                         node.InnerText,
                          item.HttpUrl + node.GetAttributeValue("href", ""),
                          item.NewsOrigin.GetEnumText(),
                          item.HttpUrl
                         );                     
                        break;
                    case NewsOrigin.游侠咨询:
                        node = document.DocumentNode.SelectNodes("//h2[@class='lone_t']/a")[0];
                        news = new News(
                        node.InnerText,
                         node.GetAttributeValue("href", ""),
                         item.NewsOrigin.GetEnumText(),
                         item.HttpUrl
                        ); ;
                        break;
                    case NewsOrigin.搜狗微信:
                        node = document.DocumentNode.SelectNodes("//div[@class='txt-box']/h3/a")[0];
                        news = new News(
                         node.InnerText,
                          node.GetAttributeValue("href", ""),
                          item.NewsOrigin.GetEnumText(),
                          item.HttpUrl
                         ); ;
                        break;
                    case NewsOrigin.CSDN:
                        string href = document.DocumentNode.SelectNodes("//div[@class='carousel-inner']/div/a")[0].GetAttributeValue("href", "");
                        string title = document.DocumentNode.SelectNodes("//div[@class='carousel-inner']/div/a/div")[0].InnerText;
                        news = new News(
                        title,
                        href,
                         item.NewsOrigin.GetEnumText(),
                         item.HttpUrl
                        ); ;
                        break;
                    case NewsOrigin.新浪新闻:
                        node = document.DocumentNode.SelectNodes("//div[@class='blk122']/a")[0];
                        //News news = new News(
                        // title,
                        // href,
                        //  item.NewsOrigin.GetEnumText(),
                        //  item.HttpUrl
                        // ); ;
                        break;
                }
                newsList.Add(news);
            }
            _newsRepository.InsertOrUpdate(newsList);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await StartNewsJob();
        }
    }
}
