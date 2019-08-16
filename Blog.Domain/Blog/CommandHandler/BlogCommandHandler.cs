using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class BlogCommandHandler : IRequestHandler<CreateBlogCommand>
    {
        private readonly IBlogRepository _blogRepository;
        public BlogCommandHandler(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public Task<Unit> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            if(request.Blog.BlogType==Core.BlogType.微语)
                _blogRepository.InsertWhisper(request.Blog);
            else
                _blogRepository.InsertArticle(request.Blog);
            return Task.FromResult(new Unit());
        }
    }
}
