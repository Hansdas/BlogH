using Blog.Application.IService;
using Blog.Application.ViewMode;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Application.Service
{
    public class TidingsService : ITidingsService
    {
        private ITidingsRepository _tidingsRepository;
        private IUserRepository _userRepository;
        public TidingsService(ITidingsRepository tidingsRepository, IUserRepository userRepository)
        {
            _tidingsRepository = tidingsRepository;
            _userRepository = userRepository;
        }
        public int SelectCountByAccount(string account)
        {
          return  _tidingsRepository.SelectCountByAccount(account);
        }
        public IList<TidingsModel> SelectByPage(int pageIndex, int pageSize, TidingsCondition TidingsCondition = null)
        {
            IList<Tidings> tidingsList = _tidingsRepository.SelectByPage(pageIndex, pageSize, TidingsCondition);
            List<string> accounts = new List<string>();
            accounts.AddRange(tidingsList.Select(s => s.PostUser));
            accounts.AddRange(tidingsList.Select(s => s.ReviceUser));
           Dictionary<string,string> dic=_userRepository.SelectNameWithAccountDic(accounts.Distinct());
            IList<TidingsModel> tidingsModels = new List<TidingsModel>();
            foreach(var item in tidingsList)
            {
                TidingsModel tidingsModel = new TidingsModel();
                tidingsModel.Content = item.AdditionalData;
                tidingsModel.IsRead = item.IsRead;
                tidingsModel.PostContent = item.PostContent;
                tidingsModel.PostUsername = dic[item.PostUser];
                tidingsModel.ReviceUsername = dic[item.ReviceUser];
                tidingsModel.Url = item.Url;
                tidingsModel.PostDate = item.SendDate.ToString("yyyy-MM-dd hh:mm");
                tidingsModels.Add(tidingsModel);
            }
            return tidingsModels;
        }
    }
}
