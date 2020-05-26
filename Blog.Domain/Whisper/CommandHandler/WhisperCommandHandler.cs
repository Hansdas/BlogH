using Blog.Common;
using Blog.Common.CacheFactory;
using Blog.Common.Json;
using Blog.Common.Socket;
using Blog.Domain.Core.Event;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class WhisperCommandHandler : ICommandHandler<CreateWhisperCommand>
    {
        private IWhisperRepository _whisperRepository;
        private ICacheClient _cacheClient;
        private ISingalrContent _singalrContent;
        private IUserRepository _userRepository;
        public WhisperCommandHandler(IWhisperRepository whisperRepository,ICacheClient cacheClient, ISingalrContent singalrContent
            , IUserRepository userRepository)
        {
            _whisperRepository = whisperRepository;
            _cacheClient = cacheClient;
            _singalrContent = singalrContent;
            _userRepository = userRepository;
        }

        public void Handler(CreateWhisperCommand command)
        {
            int id= _whisperRepository.Insert(command.Whisper);
            User user = _userRepository.SelectUserByAccount(command.Whisper.Account);
            Whisper whisper = new Whisper(
                id,
                command.Whisper.Account,
                user.Username,
                command.Whisper.Content,
                command.Whisper.IsPassing,
                command.Whisper.CommentGuids,
                DateTime.Now);
            long length =  _cacheClient.AddListTop(ConstantKey.CACHE_SQUARE_WHISPER, whisper).Result;
            int listLength = Convert.ToInt32(length);
            if (listLength > 12)
                _cacheClient.listPop(ConstantKey.CACHE_SQUARE_WHISPER);
            JsonSerializerSettings jsonSerializerSettings = new JsonContractResolver().SetJsonSerializerSettings();
            List<Whisper> whispers= _cacheClient.ListRange<Whisper>(ConstantKey.CACHE_SQUARE_WHISPER,0,5,jsonSerializerSettings).GetAwaiter().GetResult();
            Message message = new Message();
            message.Data = whispers;
            _singalrContent.SendAllClientsMessage(message);
        }                            
    }
}
