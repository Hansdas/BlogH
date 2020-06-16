using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface IWhisperRepository : IRepository<Whisper, int>
    {
        /// <summary>
        /// 发表微语
        /// </summary>
        int Insert(Whisper whisper);
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <returns></returns>
        Whisper SelectById(int id);
        /// <summary>
        /// 分页查询微语
        /// </summary>
        /// <returns></returns>
        IEnumerable<Whisper> SelectByPage(int pageIndex, int pageSize, WhisperCondiiton condiiton = null);
        /// <summary>
        ///查询总数
        /// </summary>
        /// <returns></returns>
        int SelectCount(WhisperCondiiton condiiton=null);
        /// <summary>
        /// 根据微语获取评论
        /// </summary>
        /// <param name="whisper"></param>
        /// <returns></returns>
        IList<Comment> SelectCommnetsByWhisper(int whisperId);
        /// <summary>
        /// 插入评论
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="whisperId"></param>
        void InsertComment(Comment comment, int whisperId, IList<string> whisperCommentGuids);
        /// <summary>
        /// 查询评论id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<string> SelectCommentIds(int id);
    }
}
