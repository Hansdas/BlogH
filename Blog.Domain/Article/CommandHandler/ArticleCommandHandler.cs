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
        public Task<Unit> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
