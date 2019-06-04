using Domain.Attr;

namespace Domain
{
    [Table("t_user")]
    public  class User: DomainBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string  Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }
        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsValid { get; set; }

    }

}
