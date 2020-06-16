using Blog.Common;
using Blog.Common.CacheFactory;
using Blog.Common.Socket;
using Blog.Domain.Core.Event;
using Blog.Domain.Core.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Blog.Common.Json;
using System.Linq;
using Blog.Domain.Core;

namespace Blog.Domain
{
    /// <summary>
    /// 评论触发事件处理
    /// </summary>
    public class ReviewWhisperEventHandler : IEventHandler<ReviewWhiperEvent>
    {
        private ICommentRepository _commentRepository;
        private ITidingsRepository _tidingsRepository;
        private IWhisperRepository _whisperRepository;
        private ISingalrContent _singalrContent;
        private ICacheClient _cacheClient;
        public ReviewWhisperEventHandler(ICommentRepository commentRepository, ITidingsRepository tidingsRepository, IWhisperRepository whisperRepository
            , ISingalrContent singalrContent, ICacheClient cacheClient)
        {
            _commentRepository = commentRepository;
            _tidingsRepository = tidingsRepository;
            _whisperRepository = whisperRepository;
            _singalrContent = singalrContent;
            _cacheClient = cacheClient;
        }
        /// <summary>
        /// 触发评论事件
        /// </summary>
        /// <param name="notification"></param>
        public void Handler(ReviewWhiperEvent reviewEvent)
        {
            Tidings tidings = null;
            string url = "../wshiper/detail.html?id=" + reviewEvent.WhiperId;
            if (reviewEvent.Comment.CommentType==CommentType.微语)
            {
                Whisper whisper = _whisperRepository.SelectById(reviewEvent.WhiperId);
                tidings = new Tidings(reviewEvent.Comment.Guid, reviewEvent.Comment.PostUser, reviewEvent.Comment.Content, whisper.Account, false, url, whisper.Content, DateTime.Now);
            }
            else//回复评论 
            {
                Comment comment = _commentRepository.SelectById(reviewEvent.Comment.AdditionalData);//被评论的数据;
                tidings = new Tidings(reviewEvent.Comment.Guid, reviewEvent.Comment.PostUser, reviewEvent.Comment.Content.Substring(0, 200)
                    , comment.PostUser, false, url, comment.Content, DateTime.Now);
            }
            _tidingsRepository.Insert(tidings);
            JsonSerializerSettings jsonSerializerSettings = new JsonContractResolver().SetJsonSerializerSettings();
            List<Whisper> whispers = _cacheClient.ListRange<Whisper>(ConstantKey.CACHE_SQUARE_WHISPER, 0, 5, jsonSerializerSettings).GetAwaiter().GetResult();

            IList<Comment> comments = _commentRepository.SelectByIds(reviewEvent.WhisperCommentGuids);
            int index = 0;
            Whisper cacheWhisper = null;
            for (int i = 0; i < whispers.Count; i++)
            {
                if (whispers[i].Id != reviewEvent.WhiperId)
                    continue;
                index = i;
                cacheWhisper = new Whisper(
                 whispers[i].Id,
                 whispers[i].Account,
                 whispers[i].AccountName,
                 whispers[i].Content,
                 whispers[i].IsPassing,
                 comments,
                 Convert.ToDateTime(whispers[i].CreateDate));
            }
            if (cacheWhisper == null)
                return;
            _cacheClient.ListInsert(ConstantKey.CACHE_SQUARE_WHISPER, index, cacheWhisper);
            int count = _tidingsRepository.SelectCountByAccount(reviewEvent.Comment.RevicerUser);
            Message message = new Message();
            message.Data = whispers;
            _singalrContent.SendClientMessage(reviewEvent.Comment.RevicerUser,message);
        }
    }
}
