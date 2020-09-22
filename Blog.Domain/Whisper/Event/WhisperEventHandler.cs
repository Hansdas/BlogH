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
using System.Threading.Tasks;

namespace Blog.Domain
{
    /// <summary>
    /// 评论触发事件处理
    /// </summary>
    public class WhisperEventHandler : IEventHandler<ReviewWhiperEvent>,IEventHandler<DeleteWhisperEvent>
    {
        private ICommentRepository _commentRepository;
        private ITidingsRepository _tidingsRepository;
        private IWhisperRepository _whisperRepository;
        private ISingalrContent _singalrContent;
        private ICacheClient _cacheClient;
        public WhisperEventHandler(ICommentRepository commentRepository, ITidingsRepository tidingsRepository, IWhisperRepository whisperRepository
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
            try
            {
                Tidings tidings = null;
                string url = "../whisper/whisper.html?id=" + reviewEvent.WhiperId+"&read=1";
                if (reviewEvent.Comment.CommentType == CommentType.微语)
                {
                    Whisper whisper = _whisperRepository.SelectById(reviewEvent.WhiperId);
                    tidings = new Tidings(reviewEvent.Comment.Guid, reviewEvent.Comment.PostUser, reviewEvent.Comment.Content, whisper.Account, false, url, whisper.Content, DateTime.Now);
                }
                else//回复评论 
                {
                    Comment comment = _commentRepository.SelectById(reviewEvent.Comment.AdditionalData);//被评论的数据;
                    tidings = new Tidings(reviewEvent.Comment.Guid, reviewEvent.Comment.PostUser, reviewEvent.Comment.Content
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
                     string.Join(",", comments.Select(s => s.Guid)),
                     comments,
                     Convert.ToDateTime(whispers[i].CreateDate));
                    whispers[i] = cacheWhisper;
                }
                if (cacheWhisper == null)
                    return;
                _cacheClient.ListInsert(ConstantKey.CACHE_SQUARE_WHISPER, index, cacheWhisper);
                int count = _tidingsRepository.SelectCountByAccount(reviewEvent.Comment.RevicerUser);
                Message message = new Message();
                message.Data = count;
                _singalrContent.SendClientMessage(reviewEvent.Comment.RevicerUser, message);
                //首页微语
                message.Data = whispers;
                _singalrContent.SendAllClientsMessage(message);

            }
            catch (AggregateException ex)
            {
                new LogUtils().LogError(ex, "Blog.Domain.ReviewWhisperEventHandler", ex.Message, reviewEvent.Comment.PostUser);
            }

        }

        public void Handler(DeleteWhisperEvent eventData)
        {
            _cacheClient.Remove(ConstantKey.CACHE_SQUARE_WHISPER);
             IList<Whisper> whispers = _whisperRepository.SelectByPage(1, 12).ToList();
            foreach (var item in whispers)
            {
                 _cacheClient.AddListTail(ConstantKey.CACHE_SQUARE_WHISPER, item);
            }
            whispers = whispers.Take(6).ToList();
            Message message = new Message();
            //首页微语
            message.Data = whispers;
            _singalrContent.SendAllClientsMessage(message);
        }
    }
}
