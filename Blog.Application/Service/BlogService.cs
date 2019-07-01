using Blog.Application.ViewModel;
using Blog.Domain;
using Blog.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Application
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private IUserRepository _userRepository;
        public BlogService(IBlogRepository blogRepository,IMediatorHandler mediatorHandler, IUserRepository userRepository)
        {
            _blogRepository = blogRepository;
            _mediatorHandler = mediatorHandler;
            _userRepository = userRepository;
        }

        public IList<BlogModel> GetBlogModels(int pageIndex, int pageSize, out int recordCount)
        {
            IEnumerable<Domain.Blog> blogs = _blogRepository.SelectByPage(pageIndex,pageSize,out recordCount);
            Dictionary<string, string> accountAndName = _userRepository.SelectUserByAccounts(blogs.Select(s=>s.Account).ToList());
            IList<BlogModel> blogModels = new List<BlogModel>();
            foreach (var item in blogs)
            {
                BlogModel blogModel = new BlogModel();
                blogModel.Author = accountAndName[item.Account];
                Whisper whisper = (Whisper)item.BlogBase;
                blogModel.Content = whisper.Content;
                blogModel.PhotoPath = "";
                blogModel.Reply = whisper.CommentCount;
                blogModel.Praise = whisper.PraiseCount;
                blogModel.Date = whisper.CreateTime.ToString("yyyy/MM/dd hh:mm");
                blogModel.PhotoPath = "";
                IEnumerable<CommentDataModel> commentDataModels = from comment in whisper.CommentList
                                                                  select GetCommentDataModel(comment);
                blogModel.Commentdatas = commentDataModels.ToList();
                blogModels.Add(blogModel);
            }
            return blogModels;
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
        public void PublishBlog(Domain.Blog blog)
        {
            try
            {
                var command = new CreateBlogCommand(blog);
                _mediatorHandler.SendCommand(command);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
