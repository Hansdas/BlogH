using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class CreateWhisperCommand:Command
    {
        public Whisper Whisper { get; private set; }
        public CreateWhisperCommand(Whisper whisper)
        {
            Whisper = whisper;
        }
    }
}
