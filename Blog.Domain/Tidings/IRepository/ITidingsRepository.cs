﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface ITidingsRepository
    {
        void Insert(Tidings tidings);

        int SelectCountByAccount(string account);
    }
}