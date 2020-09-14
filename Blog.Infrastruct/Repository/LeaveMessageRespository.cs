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

        public IEnumerable<leaveMessage> SelectByPage(int currentPage, int pageSize)
        {
            int pageId = pageSize * (currentPage - 1);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("pageId", pageId);
            parameters.Add("pageSize", pageSize);
            string sql = "SELECT * FROM T_LeaveMessage ORDER BY lm_createtime DESC LIMIT @pageId,@pageSize";
            IEnumerable<dynamic> dynamics = Select(sql, parameters);
            IList<leaveMessage> leaveMessages = new List<leaveMessage>();
            foreach(var item in dynamics)
            {
                leaveMessage leaveMessage = new leaveMessage(
                    item.lm_id,
                    item.lm_content,
                    item.lm_contract_email,
                    item.lm_is_action==0?false:true,
                    item.lm_createtime
                    );
                leaveMessages.Add(leaveMessage);
            }
            return leaveMessages;
        }

        public int SelectCount()
        {
            string sql = "SELECT COUNT(*) FROM T_LeaveMessage ";
            int count = SelectCount(sql,null);
            return count;
        }
    }
}
