using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class WhisperCommandHandler : IRequestHandler<CreateWhisperCommand>
    {
        private readonly IWhisperRepository _whisperRepository;
        public WhisperCommandHandler(IWhisperRepository whisperRepository)
        {
            _whisperRepository = whisperRepository;
        }
        public Task<Unit> Handle(CreateWhisperCommand request, CancellationToken cancellationToken)
        {
            _whisperRepository.Insert(request.Whisper);
            return Task.FromResult(new Unit());
        }
    }
}
