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
        public WhisperService(IWhisperRepository whisperRepository)
        {
            _whisperRepository = whisperRepository;
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
                model.PhotoPaths = item.UploadFileList.Select(s => s.SaveFullPath).ToList();
                model.Reply = item.CommentCount;
                model.Praise = item.PraiseCount;
                model.Date = item.CreateTime.ToString("yyyy/MM/dd hh:mm");
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
            commentDataModel.CommentContent = comment.CommentContent;
            commentDataModel.CommentUser = comment.CommentUsername;
            commentDataModel.UserPhotoPath = "";
            commentDataModel.CommentDate = "";
            return commentDataModel;
        }
    }
}
