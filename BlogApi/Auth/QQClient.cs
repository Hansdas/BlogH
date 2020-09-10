using Blog.Application.ViewModel;
using Blog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlogApi
{
    public class QQClient
    {
        public static string appId = "";
        public static string appKey = "";
        public static string redirectUrl = "http%3a%2f%2fwww.ttblog.site%2fhome%2findex.html";
        private class AccessTokenResult
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
        }
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        public static async Task<string> GetAccessToken(string authorizationCode)
        {
            string url = string.Format("https://graph.qq.com/oauth2.0/token?" +
                  "grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&redirect_uri={3}&fmt=json", appId, appKey, authorizationCode, redirectUrl);
            HttpClient httpClient = new HttpClient();
            string response = await httpClient.GetStringAsync(url);
            dynamic result = JsonHelper.DeserializeObject(response);
            return result.access_token;

        }
        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        public static async Task<string> GetOpenId(string accessToken)
        {
            string url = "https://graph.qq.com/oauth2.0/me?fmt=json&access_token=" + accessToken;
            HttpClient httpClient = new HttpClient();
            string response = await httpClient.GetStringAsync(url);
            dynamic result = JsonHelper.DeserializeObject(response);
            return result.openid;

        }
        /// <summary>
        /// 获取qq信息
        /// </summary>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        public static async Task<dynamic> GetQQUser(string accessToken,string openId)
        {
            string url = string.Format("https://graph.qq.com/user/get_user_info" +
                 "?access_token={0}&openid={1}&appid={2}&fmt=json", accessToken, openId, appId);
            HttpClient httpClient = new HttpClient();
            string response = await httpClient.GetStringAsync(url);
            dynamic qqInfo = JsonHelper.DeserializeObject(response);
            UserModel userModel = new UserModel();
            userModel.
            return qqInfo;

        }
    }
}
