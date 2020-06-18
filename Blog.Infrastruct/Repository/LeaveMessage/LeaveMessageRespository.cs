using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace Blog.Infrastruct
{
    public class LeaveMessageRespository : Repository, ILeaveMessageRespository
    {
        public void Insert(leaveMessage leaveMessage)
        {
            string sql = "INSERT INTO T_LeaveMessage(lm_content,lm_contract_email,lm_is_action,lm_createtime) VALUES(@Content,@ContractEmail,@IsAction,NOW())";
            DbConnection.Execute(sql,leaveMessage);
        }
    }
}
