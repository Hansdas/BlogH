using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class ReviewEvent:EventData
    {
        public ReviewEvent(Comment comment, int articleId)
        {
            Comment = comment;
            ArticleId = articleId;
        }
        public Comment Comment;
        public int  ArticleId { get; private set; }
    }
}
