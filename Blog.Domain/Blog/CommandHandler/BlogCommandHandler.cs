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
            _blogRepository.InsertWhisper(request.Blog);
            return Task.FromResult(new Unit());
        }
    }
}
