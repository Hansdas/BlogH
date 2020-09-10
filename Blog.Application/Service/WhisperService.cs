using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.CacheFactory;
using Blog.Common.Json;
using Blog.Domain;
using Blog.Domain.Core;
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
        private ICommentRepository _commentRepository;
        private IEventBus _eventBus;
        private ICacheClient _cacheClient;
        public WhisperService(IWhisperRepository whisperRepository, IEventBus eventBus
            , ICacheClient cacheClient,ICommentRepository commentRepository)
        {
            _whisperRepository = whisperRepository;
            _eventBus = eventBus;
            _cacheClient = cacheClient;
            _commentRepository = commentRepository;
        }
        public void Insert(Whisper whisper)
        {
            CreateWhisperCommand command = new CreateWhisperCommand(whisper);
            _eventBus.Publish(command); 
        }
        public IList<WhisperModel> SelectByPage(int pageIndex, int pageSize, WhisperConditionModel condiiton = null)
        {
            IEnumerable<Whisper> whispers = _whisperRepository.SelectByPage(pageIndex, pageSize, ConvertCondition(condiiton));
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
        public IList<CommentModel> SelectCommnetsByWhisper(int whisperId)
        {
            IList<Comment> comments = _whisperRepository.SelectCommnetsByWhisper(whisperId);
            return CommentModel.ConvertToCommentModels(comments);
        }
        public void Review(CommentModel commentModel, int whisperId)
        {
            string guid = Guid.NewGuid().ToString();
            Comment comment = new Comment(guid,
                commentModel.Content,
                Enum.Parse<CommentType>(commentModel.CommentType.ToString()),
                commentModel.PostUser,
                commentModel.Revicer,
                commentModel.AdditionalData);
            WhisperCommentCommand command = new WhisperCommentCommand(comment,whisperId);
          _eventBus.Publish(command);
        }

        public void DeleteById(int id)
        {
            DeleteWhisperCommand deleteWhisperCommand = new DeleteWhisperCommand(id);
            _eventBus.Publish(deleteWhisperCommand);
        }
        #region private method
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
                    IEnumerable<CommentModel> commentDataModels = CommentModel.ConvertToCommentModels(item.CommentList);
                    model.Commentdatas = commentDataModels.ToList();
                }
                whisperModels.Add(model);
            }
            return whisperModels;
        }

        private WhisperCondiiton ConvertCondition(WhisperConditionModel whisperCondition)
        {
            WhisperCondiiton condiiton = new WhisperCondiiton();
            condiiton.Account = whisperCondition.Account;
            if(whisperCondition.Id!=0)
                condiiton.Id = whisperCondition.Id;
            return condiiton;
        }

        public int SelectCount(WhisperConditionModel conditionModel = null)
        {
            WhisperCondiiton condiiton = ConvertCondition(conditionModel);
            return _whisperRepository.SelectCount(condiiton);
        }

        #endregion
    }
}
