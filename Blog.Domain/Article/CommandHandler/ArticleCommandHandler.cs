using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class ArticleCommandHandler : IRequestHandler<CreateArticleCommand>
    {
        private readonly IArticleRepository _articleRepository;
        public ArticleCommandHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public Task<Unit> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            _articleRepository.Insert(request.Article);
            return Task.FromResult(new Unit());
        }
    }
}
