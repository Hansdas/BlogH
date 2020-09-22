using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public class VideoCommandHandler : ICommandHandler<CreateVideoCommand>
    {
        private IVideoRepository _videoRepository;
        public VideoCommandHandler(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }
        public void Handler(CreateVideoCommand command)
        {
            _videoRepository.Insert(command.Video);
        }
    }
}
