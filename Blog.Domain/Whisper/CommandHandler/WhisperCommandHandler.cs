using Blog.Domain.Core.Event;

namespace Blog.Domain
{
    public class WhisperCommandHandler : ICommandHandler<CreateWhisperCommand>
    {
        private readonly IWhisperRepository _whisperRepository;
        public WhisperCommandHandler(IWhisperRepository whisperRepository)
        {
            _whisperRepository = whisperRepository;
        }

        public void Handler(CreateWhisperCommand command)
        {
            _whisperRepository.Insert(command.Whisper);
        }
    }
}
