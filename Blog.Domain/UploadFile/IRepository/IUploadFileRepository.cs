using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface IUploadFileRepository
    {
        IList<UploadFile> SelectByIds(IList<string> guids);
    }
}
