using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class ReviewWhiperEvent:EventData
    {
        public ReviewWhiperEvent(Comment comment, int whisper, IList<string> whisperCommentGuids)
        {
            Comment = comment;
            WhiperId = whisper;
            WhisperCommentGuids = whisperCommentGuids;
        }
        public Comment Comment;
        public int  WhiperId { get; private set; }
        public IList<string> WhisperCommentGuids { get; private set; }
    }
}
