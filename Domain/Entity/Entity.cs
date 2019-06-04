using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entity
{
   public abstract class Entity
    {
        /// <summary>
        /// 自增id
        /// </summary>
        int Id { get; set; }
    }
}
