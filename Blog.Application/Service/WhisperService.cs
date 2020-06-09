using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.CacheFactory;
using Blog.Common.Json;
using Blog.Domain;
using Blog.Domain.Core.Bus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application
{
    public class WhisperService : IWhisperService
    {
        private  IWhisperRepository _whisperRepository;
        private IEventBus _eventBus;
        private ICacheClient _cacheClient;
        public WhisperService(IWhisperRepository whisperRepository, IEventBus eventBus
            , ICacheClient cacheClient)
        {
            _whisperRepository = whisperRepository;
            _eventBus = eventBus;
            _cacheClient = cacheClient;
        }
        public void Insert(Whisper whisper)
        {
            CreateWhisperCommand command = new CreateWhisperCommand(whisper);
            _eventBus.Publish(command); 
        }
        public IList<WhisperModel> SelectByPage(int pageIndex, int pageSize, WhisperCondiiton condiiton = null)
        {
            IEnumerable<Whisper> whispers = _whisperRepository.SelectByPage(pageIndex, pageSize, condiiton);
            return ConvertToModel(whispers);
        }

        public async Task<IList<WhisperModel>> SelectByPageCache(int pageIndex, int pageSize)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonContractResolver().SetJsonSerializerSettings();
            IList<Whisper> whispers=await _cacheClient.ListRange<Whisper>(ConstantKey.CACHE_SQUARE_WHISPER, pageIndex, pageSize, jsonSerializerSettings);
            if (whispers.Count == 0)
            {
                whispers = _whisperRepository.SelectByPage(1,12).ToList();
                foreach(var item in whispers)
                {
                   await _cacheClient.AddListTail(ConstantKey.CACHE_SQUARE_WHISPER, item);
                }
                whispers = whispers.Take(6).ToList();
            }
            return ConvertToModel(whispers);
        }
        private IList<WhisperModel> ConvertToModel(IEnumerable<Whisper> whispers)
        {
            IList<WhisperModel> whisperModels = new List<WhisperModel>();
            foreach (var item in whispers)
            {                                                
                WhisperModel model = new WhisperModel();
                model.Id = item.Id.ToString();
                model.Account = item.Account;
                model.AccountName = item.AccountName;
                model.Content = item.Content;
                model.CreateDate = item.CreateTime.Value.ToString("yyyy-MM-dd hh:mm");
                if (item.CommentList != null)
                {
                    IEnumerable<CommentDataModel> commentDataModels = from comment in item.CommentList
                                                                      select GetCommentDataModel(comment);
                    model.Commentdatas = commentDataModels.ToList();
                }
                whisperModels.Add(model);
            }
            return whisperModels;
        }
        private CommentDataModel GetCommentDataModel(Comment comment)
        {
            CommentDataModel commentDataModel = new CommentDataModel();
            commentDataModel.CommentContent = comment.Content;
            commentDataModel.CommentUser = comment.PostUser;
            commentDataModel.UserPhotoPath = "";
            commentDataModel.CommentDate = "";
            return commentDataModel;
        }
    }
}
