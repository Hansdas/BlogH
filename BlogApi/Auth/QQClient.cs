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
        public static string appId = "101895784";
        public static string appKey = "3307fb54bdb6cd6d3bce96807b5c3705";
        public static string redirectUrl = "http%3a%2f%2fwww.ttblog.site%2flogin%2fcallback.html";
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        public static async Task<string> GetAccessToken(string authorizationCode)
        {
            if (string.IsNullOrEmpty(authorizationCode))
                throw new AuthException("qq认证登录失败：authorizationCode为空");
            string url = string.Format("https://graph.qq.com/oauth2.0/token?" +
                  "grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&redirect_uri={3}&fmt=json", appId, appKey, authorizationCode, redirectUrl);
            HttpClient httpClient = new HttpClient();
            string response = await httpClient.GetStringAsync(url);
            dynamic result = JsonHelper.DeserializeObject(response);
            string error = result.error;
            if (!string.IsNullOrEmpty(error))
                throw new AuthException("qq认证登录失败：" + result.error_description);
            return result.access_token;

        }
        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        public static async Task<string> GetOpenId(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new AuthException("qq认证登录失败：accessToken无效");
            string url = "https://graph.qq.com/oauth2.0/me?fmt=json&access_token=" + accessToken;
            HttpClient httpClient = new HttpClient();
            string response = await httpClient.GetStringAsync(url);
            dynamic result = JsonHelper.DeserializeObject(response);
            string error = result.error;
            if (!string.IsNullOrEmpty(error))
                throw new AuthException("qq认证登录失败："+result.error_description);
            return result.openid;

        }
        /// <summary>
        /// 获取qq信息
        /// </summary>
        /// <param name="authorizationCode"></param>
        /// <returns></returns>
        public static async Task<UserModel> GetQQUser(string accessToken,string openId)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new AuthException("accessToken无效");
            if (string.IsNullOrEmpty(openId))
                throw new AuthException("openId");
            string url = string.Format("https://graph.qq.com/user/get_user_info" +
                 "?access_token={0}&openid={1}&appid={2}&fmt=json", accessToken, openId, appId);
            HttpClient httpClient = new HttpClient();
            string response = await httpClient.GetStringAsync(url);
            dynamic result = JsonHelper.DeserializeObject(response);
            if (result.ret != 0)
                throw new AuthException("qq认证登录失败：" + result.msg);
            UserModel userModel = new UserModel();
            userModel.Account = openId;
            userModel.Username = result.nickname;
            userModel.LoginType = Blog.Domain.Core.LoginType.QQ;
            userModel.Sex = result.gender;
            userModel.HeadPhoto = result.figureurl_qq_2;
            return userModel;

        }
    }
}
