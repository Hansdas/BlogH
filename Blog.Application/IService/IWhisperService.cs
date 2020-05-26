using Blog.Application.ViewModel;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application
{
   public interface IWhisperService
    {
        void Insert(Whisper whisper);
        IList<WhisperModel> SelectByPage(int pageIndex, int pageSize, WhisperCondiiton condiiton = null);

        Task<IList<WhisperModel>> SelectByPageCache(int pageIndex, int pageSize);
    }
}
