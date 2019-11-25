using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class ArticleCommandHandler : IEventHandler<CreateArticleCommand>,IEventHandler<UpdateArticleCommand>
    {
        private readonly IArticleRepository _articleRepository;
        public ArticleCommandHandler(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public void Handler(CreateArticleCommand command)
        {
            _articleRepository.Insert(command.Article);
        }

        public void Handler(UpdateArticleCommand command)
        {
            if (command.Id > 0)
                _articleRepository.Comment(command.Comments, command.Id);
            else
                _articleRepository.Update(command.Article);
        }
    }
}
