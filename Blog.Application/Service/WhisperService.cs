using Blog.Application.ViewModel;
using Blog.Common;
using Blog.Common.CacheFactory;
using Blog.Domain;
using Blog.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Application
{
    public class WhisperService : IWhisperService
    {
        private  IWhisperRepository _whisperRepository;
        private IEventBus _eventBus;
        public WhisperService(IWhisperRepository whisperRepository, IEventBus eventBus)
        {
            _whisperRepository = whisperRepository;
            _eventBus = eventBus;
        }
        public void Insert(Whisper whisper)
        {
            CreateWhisperCommand command = new CreateWhisperCommand(whisper);
            _eventBus.Publish(command); 
        }
        public IList<WhisperModel> SelectByPage(int pageIndex, int pageSize, WhisperCondiiton condiiton = null)
        {
            IEnumerable<Whisper> whispers = _whisperRepository.SelectByPage(pageIndex, pageSize, condiiton);
            IList<WhisperModel> whisperModels = new List<WhisperModel>();
            foreach (var item in whispers)
            {
                WhisperModel model = new WhisperModel();
                model.Author = item.Account;
                model.Content = item.Content;
                model.Reply = item.CommentCount;
                model.Date = item.CreateTime.ToString("yyyy年MM月dd hh点mm分");
                IEnumerable<CommentDataModel> commentDataModels = from comment in item.CommentList
                                                                  select GetCommentDataModel(comment);
                model.Commentdatas = commentDataModels.ToList();
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
