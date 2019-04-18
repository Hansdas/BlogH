using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Attr
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = false)]
    public class TableAttribute:Attribute
    {
        public TableAttribute(string tableName)
        {
            TableName = tableName;
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
    }
}
