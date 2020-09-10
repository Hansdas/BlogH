using Blog.Domain;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Quartz
{
    public class ContainerJob : IJob
    {
        private INewsRepository _newsRepository;
        public ContainerJob(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
