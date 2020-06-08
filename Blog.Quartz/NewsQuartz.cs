using Blog.Common;
using Blog.Domain;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Quartz
{
    public class NewsQuartz
    {
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
        public async Task StartJob()
        {
            IList<NewsUrlItem> httpUrls = new List<NewsUrlItem>() {
               new NewsUrlItem("https://daily.zhihu.com",NewsOrigin.知乎) ,
                new NewsUrlItem("https://www.ali213.net/news/game",NewsOrigin.游侠咨询),
                 new NewsUrlItem("https://weixin.sogou.com",NewsOrigin.搜狗微信) ,
                   new NewsUrlItem("https://www.csdn.net",NewsOrigin.CSDN)  ,
                       new NewsUrlItem("https://news.sina.com.cn/world",NewsOrigin.新浪新闻)
            };
            foreach (var item in httpUrls)
            {
                switch (item.NewsOrigin)
                {
                    //case NewsOrigin.知乎:
                    //    HttpClient httpClient = new HttpClient();
                    //    HttpResponseMessage response = await httpClient.GetAsync(item.HttpUrl);
                    //    response.EnsureSuccessStatusCode();
                    //    string html = response.Content.ReadAsStringAsync().Result;
                    //    HtmlDocument document = new HtmlDocument();
                    //    document.LoadHtml(html);
                    //    var nodes = document.DocumentNode.SelectNodes("//a[@class='link-button']")[0];

                    //    News news = new News(
                    //     nodes.InnerText,
                    //      item.HttpUrl + nodes.GetAttributeValue("href", ""),
                    //      item.NewsOrigin.GetEnumText(),
                    //      item.HttpUrl
                    //     ); ;
                    //    break;
                    //case NewsOrigin.游侠咨询:
                    //    HttpClient httpClient = new HttpClient();
                    //    HttpResponseMessage response = await httpClient.GetAsync(item.HttpUrl);
                    //    response.EnsureSuccessStatusCode();
                    //    string html = response.Content.ReadAsStringAsync().Result;
                    //    HtmlDocument document = new HtmlDocument();
                    //    document.LoadHtml(html);
                    //    var nodes = document.DocumentNode.SelectNodes("//h2[@class='lone_t']/a")[0];
                    //    News news = new News(
                    //     nodes.InnerText,
                    //      nodes.GetAttributeValue("href", ""),
                    //      item.NewsOrigin.GetEnumText(),
                    //      item.HttpUrl
                    //     ); ;
                    //    break;
                    //case NewsOrigin.搜狗微信:
                    //    HttpClient httpClient = new HttpClient();
                    //    HttpResponseMessage response = await httpClient.GetAsync(item.HttpUrl);
                    //    response.EnsureSuccessStatusCode();
                    //    string html = response.Content.ReadAsStringAsync().Result;
                    //    HtmlDocument document = new HtmlDocument();
                    //    document.LoadHtml(html);
                    //    var nodes = document.DocumentNode.SelectNodes("//div[@class='txt-box']/h3/a")[0];
                    //    News news = new News(
                    //     nodes.InnerText,
                    //      nodes.GetAttributeValue("href", ""),
                    //      item.NewsOrigin.GetEnumText(),
                    //      item.HttpUrl
                    //     ); ;
                    //    break;
                    //case NewsOrigin.CSDN:
                    //    HttpClient httpClient = new HttpClient();
                    //    HttpResponseMessage response = await httpClient.GetAsync(item.HttpUrl);
                    //    response.EnsureSuccessStatusCode();
                    //    string html = response.Content.ReadAsStringAsync().Result;
                    //    HtmlDocument document = new HtmlDocument();
                    //    document.LoadHtml(html);
                    //    string href = document.DocumentNode.SelectNodes("//div[@class='carousel-inner']/div/a")[0].GetAttributeValue("href","");
                    //    string title = document.DocumentNode.SelectNodes("//div[@class='carousel-inner']/div/a/div")[0].InnerText;
                    //    News news = new News(
                    //     title,
                    //     href,
                    //      item.NewsOrigin.GetEnumText(),
                    //      item.HttpUrl
                    //     ); ;
                    //    break;
                    case NewsOrigin.新浪新闻:
                        HttpClient httpClient = new HttpClient();
                        HttpResponseMessage response = await httpClient.GetAsync(item.HttpUrl);
                        response.EnsureSuccessStatusCode();
                        string html = response.Content.ReadAsStringAsync().Result;
                        HtmlDocument document = new HtmlDocument();
                        document.LoadHtml(html);
                        var node = document.DocumentNode.SelectNodes("//div[@class='blk122']/a")[0];
                        //News news = new News(
                        // title,
                        // href,
                        //  item.NewsOrigin.GetEnumText(),
                        //  item.HttpUrl
                        // ); ;
                        break;
                }
            }
        }
    }
}
